using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if WINDOWS 
using CFI.Client.JobInspectionService;
#endif

namespace CFI.Client
{
	public class WebServiceAPIWrapper
	{
		private JobInspectionClient wsClient = null;
		private string userName = null;

		#region events
		public class WebServiceChannelStateChangedEventArgs : EventArgs
		{
			public WebServiceChannelState State = WebServiceChannelState.Created;
		}

		public event EventHandler<WebServiceChannelStateChangedEventArgs> ChannelStateChanged;
		private void onChannelStateChanged(WebServiceChannelState newState)
		{
			if (ChannelStateChanged != null)
			{
				WebServiceChannelStateChangedEventArgs args = new WebServiceChannelStateChangedEventArgs();
				args.State = newState;
				ChannelStateChanged(this, args);
			}
		}
		
		// file uploading events
		public event EventHandler<TransferProgressEventArgs> UploadStarted;
		private void onUploadStarted( int totalToUpload )
		{
			if ( UploadStarted != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.TotalToTransfer = totalToUpload;
				UploadStarted( this, args );
			}
		}
		public event EventHandler<TransferProgressEventArgs> PartUploaded;
		private void onPartUploaded( int numUploaded, int totalToUpload )
		{
			if ( PartUploaded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = numUploaded;
				args.TotalToTransfer = totalToUpload;
				PartUploaded( this, args );
			}
		}
		public event EventHandler<TransferProgressEventArgs> UploadEnded;
		private void onUploadEnded( int totalUploaded )
		{
			if ( UploadEnded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = totalUploaded;
				args.TotalToTransfer = totalUploaded;
				UploadEnded( this, args );
			}
		}

		// file downloading events
		public event EventHandler<TransferProgressEventArgs> DownloadStarted;
		private void onDownloadStarted( int totalToDownload )
		{
			if ( DownloadStarted != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.TotalToTransfer = totalToDownload;
				DownloadStarted( this, args );
			}
		}
		public event EventHandler<TransferProgressEventArgs> PartDownloaded;
		private void onPartDownloaded( int numDownloaded, int totalToDownload )
		{
			if ( PartDownloaded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = numDownloaded;
				args.TotalToTransfer = totalToDownload;
				PartDownloaded( this, args );
			}
		}
		public event EventHandler<TransferProgressEventArgs> DownloadEnded;
		private void onDownloadEnded( int totalDownloaded )
		{
			if ( DownloadEnded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = totalDownloaded;
				args.TotalToTransfer = totalDownloaded;
				DownloadEnded( this, args );
			}
		}
		
		#endregion
		private string accessToken()
		{
			return SecurityUtils.CreateAccessToken(this.userName);
		}

		public JobInspectionClient ServiceClient
		{
			get { return wsClient; }
		}

		internal void DebugSetUserName(string newUserName)
		{
			// This API is for the sole purpose of allowing unit tests to connect to the service as a valid user so that the channel can be opened.
			// Once the channel is open then the user name can be changed to allow testing scenarios where invalid accesstokens get sent over the wire.
			this.userName = newUserName;
		}

		public bool Connect(string url, string userName, out string errorMessage, out bool invalidUserName)
		{
			errorMessage = "";
			invalidUserName = false;
			this.userName = userName;
			if (wsClient == null)
			{
				wsClient = WebServiceUtils.CreateServiceClient(url);
				subscribeChannelEvents();

				try
				{
					// the API call forces the channel open and also validates the user token
					string text = "PING!";
					if (wsClient.Echo(accessToken(), text) != text)
					{
						errorMessage = "Echo test failed.  Reflected text did not match original.";
						return false;
					}
				}
				catch (Exception ex)
				{
					InvalidUserException iuex = TranslateException(ex) as InvalidUserException;
					if (iuex != null)
					{
						// echo failed because of invalid user
						invalidUserName = true;
						errorMessage = iuex.Message;
						return false;
					}
					else
					{
						// echo failed for some other reason
						errorMessage = string.Format("Connect Failed\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
						return false;
					}
				}
			}
			return true;
		}

		public void Disconnect()
		{
			if (wsClient != null)
			{
				try
				{
					wsClient.Close();
				}
				catch
				{
				}
				finally
				{
					unsubscribeChannelEvents();
					wsClient = null;
				}
			}
		}

		public PhotoInfo GetPhoto(int photoID)
		{
			assertConnected();
			try
			{
				return wsClient.GetPhoto(accessToken(), photoID);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public bool DeletePhoto(int photoID)
		{
			assertConnected();
			try
			{
				return wsClient.DeletePhoto(accessToken(), photoID);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public PhotoInfo[] GetPhotos(int orderID)
		{
			assertConnected();
			try
			{
				string photoListXml = wsClient.GetPhotos(accessToken(), orderID);
				return PhotoInfo.ParsePhotosXml(photoListXml);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public bool IsConnected
		{
			get
			{
				return ((wsClient != null) && 
						(wsClient.State == System.ServiceModel.CommunicationState.Opened));
			}
		}

		public WebServiceChannelState? ChannelState
		{
			get
			{
				if ( wsClient == null )
				{
					return null;
				}

				switch (wsClient.State)
				{
					case System.ServiceModel.CommunicationState.Closed:
						return WebServiceChannelState.Closed;
					case System.ServiceModel.CommunicationState.Closing:
						return WebServiceChannelState.Closing;
					case System.ServiceModel.CommunicationState.Created:
						return WebServiceChannelState.Created;
					case System.ServiceModel.CommunicationState.Faulted:
						return WebServiceChannelState.Faulted;
					case System.ServiceModel.CommunicationState.Opened:
						return WebServiceChannelState.Opened;
					case System.ServiceModel.CommunicationState.Opening:
						return WebServiceChannelState.Opening;
					default:
						throw new ApplicationException("unexpected Channel State enum encountered");
				}
			}
		}

		public string Echo(string returnThis)
		{
			assertConnected();
			try
			{
				return wsClient.Echo(accessToken(), returnThis);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public int[] GetOrderIDsByCustomerLastName(string lastName, bool scheduledOnly, bool activeOnly, int maxOrders)
		{
			assertConnected();
			try
			{
				return wsClient.GetOrderIDsByCustomerLastName(accessToken(), lastName, scheduledOnly, activeOnly, maxOrders);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public int[] GetOrderIDsByStoreNumber(string storeNumber, bool scheduledOnly, bool activeOnly, int maxOrders)
		{
			assertConnected();
			try
			{
				return wsClient.GetOrderIDsByStoreNumber(accessToken(), storeNumber, scheduledOnly, activeOnly, maxOrders);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public int[] GetOrderIDsByDateRange(DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders)
		{
			assertConnected();
			try
			{
				return wsClient.GetOrderIDsByDateRange(accessToken(), range, scheduledOnly, activeOnly, maxOrders);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public int[] GetOrderIDsByMultipleCriteria(string lastName, string poNumber, string storeNumber, DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders)
		{
			assertConnected();
			try
			{
				return wsClient.GetOrderIDsByMultipleCriteria(accessToken(), lastName, poNumber, storeNumber, range, scheduledOnly, activeOnly, maxOrders);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public int[] GetOrderIDs(bool scheduledOnly, bool activeOnly, int maxOrders)
		{
			assertConnected();
			try
			{
				return wsClient.GetOrderIDs(accessToken(), scheduledOnly, activeOnly, maxOrders);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public OrderInfo GetOrderByID(int ID)
		{
			assertConnected();
			try
			{
				return wsClient.GetOrderByID(accessToken(), ID);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public int[] GetOrdersByPONumber(string poNumber, bool scheduledOnly, bool activeOnly, int maxOrders)
		{
			assertConnected();
			try
			{
				return wsClient.GetOrdersByPONumber(accessToken(), poNumber, scheduledOnly, activeOnly, maxOrders);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public bool AddNote(int orderID, NoteInfo note)
		{
			assertConnected();
			try
			{
				if (wsClient.AddNote(accessToken(), orderID, note) == true)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public NoteInfo GetNote(int noteID)
		{
			assertConnected();
			try
			{
				return wsClient.GetNote(accessToken(), noteID);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public bool UpdateNoteText(int noteID, string newText)
		{
			assertConnected();
			try
			{
				return wsClient.UpdateNoteText(accessToken(), noteID, newText);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public bool DeleteNote(int noteID)
		{
			assertConnected();
			try
			{
				return wsClient.DeleteNote(accessToken(), noteID);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public NoteTypeInfo[] GetAllNoteTypes()
		{
			assertConnected();
			try
			{
				string noteTypeListXml = wsClient.GetAllNoteTypes(accessToken());
				return NoteTypeInfo.ParseNoteTypesXml(noteTypeListXml);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public UserInfo[] GetAllUsers()
		{
			assertConnected();
			try
			{
				string userListXml = wsClient.GetAllUsers(accessToken());
				return UserInfo.ParseUsersXml(userListXml);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public NoteInfo[] GetNotes(int orderID)
		{
			assertConnected();
			try
			{
				string noteListXml = wsClient.GetNotes(accessToken(), orderID);
				return NoteInfo.ParseNotesXml(noteListXml);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public byte[] DownloadDiagram( string diagramNumber )
		{
			assertConnected();
			try
			{
				// prepare for download
				string downloadFileClaimToken = wsClient.QueueDiagramDownload(accessToken(), diagramNumber);
				if (downloadFileClaimToken == null)
				{
					return null;
				}

				// download all parts
				return downloadFileIncrementally(downloadFileClaimToken);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public byte[] DownloadPhoto( int photoID )
		{
			assertConnected();
			try
			{
				// prepare photo file for download
				string downloadFileClaimToken = wsClient.QueuePhotoDownload(accessToken(), photoID);
				if (downloadFileClaimToken == null)
				{
					return null;
				}

				// download all parts
				return downloadFileIncrementally(downloadFileClaimToken);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public bool AddPhoto(int orderID, PhotoInfo photo, byte[] photoBytes, string fileExtension)
		{
			assertConnected();
			try
			{
				// The initial FilePath property of the info object is ignored.  The web service will
				// decide on the final file name

				// Upload photo
				string claimToken = uploadFileIncrementally(photoBytes);
				if (claimToken == null)
				{
					return false;
				}

				// add the photo record.  the claim token is used to allow the service to 
				if (wsClient.AddPhoto(accessToken(), orderID, photo, fileExtension, claimToken) == true)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public string[] GetLogDirectoryNames()
		{
			assertConnected();
			try
			{
				return wsClient.GetLogDirectoryNames(accessToken());
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public string GetCurrentLogDirectoryName()
		{
			assertConnected();
			try
			{
				return wsClient.GetCurrentLogDirectoryName(accessToken());
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public string[] GetLogFileNames(string logDirectoryName)
		{
			assertConnected();
			try
			{
				return wsClient.GetLogFileNames(accessToken(), logDirectoryName);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		public byte[] DownloadLogFile(string directoryName, string fileName)
		{
			assertConnected();
			try
			{
				// make sure that only a terminal folder name and a not-fully-qualified filename are passed to the server
				fileName = fileName.Replace("\\", "/");
				fileName = fileName.Substring(fileName.LastIndexOf("/") + 1);

				directoryName = directoryName.Replace("\\", "/");
				directoryName = directoryName.Substring(directoryName.LastIndexOf("/") + 1);

				// prepare for download
				string downloadFileClaimToken = wsClient.QueueLogFileDownload(accessToken(), directoryName, fileName);
				if (downloadFileClaimToken == null)
				{
					return null;
				}

				// download all parts
				return downloadFileIncrementally(downloadFileClaimToken);
			}
			catch (Exception ex)
			{
				throw TranslateException(ex);
			}
		}

		private void subscribeChannelEvents()
		{
			if (wsClient.InnerChannel != null)
			{
				wsClient.InnerChannel.Closed += new EventHandler(InnerChannel_Closed);
				wsClient.InnerChannel.Closing += new EventHandler(InnerChannel_Closing);
				wsClient.InnerChannel.Faulted += new EventHandler(InnerChannel_Faulted);
				wsClient.InnerChannel.Opened += new EventHandler(InnerChannel_Opened);
				wsClient.InnerChannel.Opening += new EventHandler(InnerChannel_Opening);
			}
		}

		private void unsubscribeChannelEvents()
		{
			try
			{
				if ((wsClient != null) && (wsClient.InnerChannel != null))
				{
					wsClient.InnerChannel.Closed -= new EventHandler(InnerChannel_Closed);
					wsClient.InnerChannel.Closing -= new EventHandler(InnerChannel_Closing);
					wsClient.InnerChannel.Faulted -= new EventHandler(InnerChannel_Faulted);
					wsClient.InnerChannel.Opened -= new EventHandler(InnerChannel_Opened);
					wsClient.InnerChannel.Opening -= new EventHandler(InnerChannel_Opening);
				}
			}
			catch
			{
			}
		}

		private void InnerChannel_Opening(object sender, EventArgs e)
		{
			onChannelStateChanged(WebServiceChannelState.Opening);
		}

		private void InnerChannel_Opened(object sender, EventArgs e)
		{
			onChannelStateChanged(WebServiceChannelState.Opened);
		}

		private void InnerChannel_Faulted(object sender, EventArgs e)
		{
			onChannelStateChanged(WebServiceChannelState.Faulted);
		}

		private void InnerChannel_Closing(object sender, EventArgs e)
		{
			onChannelStateChanged(WebServiceChannelState.Closing);
		}

		private void InnerChannel_Closed(object sender, EventArgs e)
		{
			onChannelStateChanged(WebServiceChannelState.Closed);
		}

		private void assertConnected()
		{
			if ((wsClient == null) || (wsClient.State != System.ServiceModel.CommunicationState.Opened))
			{
				throw new WebServiceNotConnectedException("Cannot call Web Service APIs when service channel is not connected");
			}
		}

		
		// returns a string to use as a file claim token
		private string uploadFileIncrementally(byte[] bytes)
		{
			int chunkSize = 16000;
			
			onUploadStarted(bytes.Length);
			string claimToken = wsClient.UploadFileStart(accessToken(), bytes.Length, chunkSize);
			int numChunks = bytes.Length / chunkSize;
			int fragmentChunkSize = bytes.Length % chunkSize;
			if ( fragmentChunkSize > 0 )
			{
				numChunks += 1;
			}

			// move the full-sized chunks
			int numUploaded = 0;
			byte[] buffer = new byte[ chunkSize ];
			for (int i = 0; i < (numChunks - 1); i++ )
			{
				Array.Copy( bytes, i * chunkSize, buffer, 0, chunkSize );
				if (wsClient.UploadFilePart(accessToken(), claimToken, i, buffer) == false)
				{
					return null;
				}
				numUploaded += chunkSize;
				onPartUploaded( numUploaded, bytes.Length );
			}

			// move the fragment chunk (if any)
			if ( fragmentChunkSize > 0 )
			{
				buffer = new byte[ fragmentChunkSize ];
				Array.Copy(bytes, (numChunks - 1) * chunkSize, buffer, 0, fragmentChunkSize);
				if (wsClient.UploadFilePart(accessToken(), claimToken, numChunks - 1, buffer) == false)
				{
					return null;
				}
				numUploaded += fragmentChunkSize;
				onPartUploaded( numUploaded, bytes.Length );
			}

			// end the upload
			if (wsClient.UploadFileEnd(accessToken(), claimToken) == false)
			{
				return null;
			}
			onUploadEnded( bytes.Length );

			return claimToken;
		}

		private byte[] downloadFileIncrementally(string claimToken)
		{
			int chunkSize = 16000;
			
			int totalBytes = wsClient.DownloadFileStart(accessToken(), claimToken, chunkSize);
			onDownloadStarted( totalBytes );
			if ( totalBytes == 0 )
			{
				return null;
			}
			int numChunks = totalBytes / chunkSize;
			int fragmentChunkSize = totalBytes % chunkSize;
			if (fragmentChunkSize > 0)
			{
				numChunks += 1;
			}

			// create the buffer
			byte[] buffer = new byte[ totalBytes ];
			int numDownloaded = 0;
			// move the full-sized chunks
			for (int i = 0; i < (numChunks - 1); i++)
			{
				byte[] chunk = wsClient.DownloadFilePart(accessToken(), claimToken, i);
				if ( chunk == null )
				{
					return null;
				}
				Array.Copy(chunk, 0, buffer, (chunkSize * i), chunkSize);
				numDownloaded += chunkSize;
				onPartDownloaded( numDownloaded, totalBytes );
			}

			// move the fragment chunk (if any)
			if (fragmentChunkSize > 0)
			{
				byte[] chunk = wsClient.DownloadFilePart(accessToken(), claimToken, numChunks - 1);
				if (chunk == null)
				{
					return null;
				}
				Array.Copy(chunk, 0, buffer, (numChunks - 1) * chunkSize, fragmentChunkSize);
				numDownloaded += fragmentChunkSize;
				onPartDownloaded( numDownloaded, totalBytes );
			}

			// end the download
			if (wsClient.DownloadFileEnd(accessToken(), claimToken) == false)
			{
				return null;
			}
			onDownloadEnded(totalBytes);
			
			return buffer;
		}

		public static Exception TranslateException(Exception ex)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				Exception currentException = ex;
				while (currentException != null)
				{
					sb.AppendFormat("{0}\r\n", currentException.Message);
					currentException = currentException.InnerException;
				}
				string searchText = sb.ToString();
				if (searchText.Contains(SecurityUtils.InvalidUserNameMagicTextToken) == true)
				{
					return new InvalidUserException("[Aggregated Message] - " + searchText, ex);
				}

				// if we got to here there is no known translation
				return ex;
			}
			catch
			{
				return ex;
			}
		}
	}

	public class TransferProgressEventArgs : EventArgs
	{
		public int NumTransferred = 0;
		public int TotalToTransfer= 0;
	}
	
	
	public class InvalidUserException : Exception
	{
		private InvalidUserException()
		{
		}

		public InvalidUserException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public InvalidUserException(string message)
			: base(message, null)
		{
		}

	}

}
