using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;
using System.Data;
using CFI;

#if WINDOWS
    using System.Net.NetworkInformation;
#endif


namespace CFI.Client
{
    public class CFIClient : IDisposable
    {
        private string lastConnectionUrl;
        //private bool lastConnectionSucceeded;
        private string userName;
        private bool initialized = false;
        private WebServiceAPIWrapper wsAPI;
        private Cache cache;

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

		// note uploading events
		public event EventHandler<TransferProgressEventArgs> NotesUploadStarted;
		private void onNotesUploadStarted( int totalToUpload )
		{
			if ( NotesUploadStarted != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.TotalToTransfer = totalToUpload;
				NotesUploadStarted( this, args );
			}
		}
		
		public event EventHandler<TransferProgressEventArgs> NoteUploaded;
		private void onNoteUploaded( int numUploaded, int totalToUpload )
		{
			if ( NoteUploaded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = numUploaded;
				args.TotalToTransfer = totalToUpload;
				NoteUploaded( this, args );
			}
		}
		
		public event EventHandler<TransferProgressEventArgs> NotesUploadEnded;
		private void onNotesUploadEnded( int totalUploaded )
		{
			if ( NotesUploadEnded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = totalUploaded;
				args.TotalToTransfer = totalUploaded;
				NotesUploadEnded( this, args );
			}
		}

		// note downloading events
		public event EventHandler<TransferProgressEventArgs> NotesDownloadStarted;
		private void onNotesDownloadStarted( int totalToDownload )
		{
			if ( NotesDownloadStarted != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.TotalToTransfer = totalToDownload;
				NotesDownloadStarted( this, args );
			}
		}
		
		public event EventHandler<TransferProgressEventArgs> NoteDownloaded;
		private void onNoteDownloaded( int numDownloaded, int totalToDownload )
		{
			if ( NoteDownloaded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = numDownloaded;
				args.TotalToTransfer = totalToDownload;
				NoteDownloaded( this, args );
			}
		}
		
		public event EventHandler<TransferProgressEventArgs> NotesDownloadEnded;
		private void onNotesDownloadEnded( int totalDownloaded )
		{
			if ( NotesDownloadEnded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = totalDownloaded;
				args.TotalToTransfer = totalDownloaded;
				NotesDownloadEnded( this, args );
			}
		}

		// Photo uploading events
		public event EventHandler<TransferProgressEventArgs> PhotosUploadStarted;
		private void onPhotosUploadStarted( int totalToUpload )
		{
			if ( PhotosUploadStarted != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.TotalToTransfer = totalToUpload;
				PhotosUploadStarted( this, args );
			}
		}
		
		public event EventHandler<TransferProgressEventArgs> PhotoUploaded;
		private void onPhotoUploaded( int numUploaded, int totalToUpload )
		{
			if ( PhotoUploaded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = numUploaded;
				args.TotalToTransfer = totalToUpload;
				PhotoUploaded( this, args );
			}
		}
		
		public event EventHandler<TransferProgressEventArgs> PhotosUploadEnded;
		private void onPhotosUploadEnded( int totalUploaded )
		{
			if ( PhotosUploadEnded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = totalUploaded;
				args.TotalToTransfer = totalUploaded;
				PhotosUploadEnded( this, args );
			}
		}

		// Photo downloading events
		public event EventHandler<TransferProgressEventArgs> PhotosDownloadStarted;
		private void onPhotosDownloadStarted( int totalToDownload )
		{
			if ( PhotosDownloadStarted != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.TotalToTransfer = totalToDownload;
				PhotosDownloadStarted( this, args );
			}
		}
		
		public event EventHandler<TransferProgressEventArgs> PhotoDownloaded;
		private void onPhotoDownloaded( int numDownloaded, int totalToDownload )
		{
			if ( PhotoDownloaded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = numDownloaded;
				args.TotalToTransfer = totalToDownload;
				PhotoDownloaded( this, args );
			}
		}
		
		public event EventHandler<TransferProgressEventArgs> PhotosDownloadEnded;
		private void onPhotosDownloadEnded( int totalDownloaded )
		{
			if ( PhotosDownloadEnded != null )
			{
				TransferProgressEventArgs args = new TransferProgressEventArgs();
				args.NumTransferred = totalDownloaded;
				args.TotalToTransfer = totalDownloaded;
				PhotosDownloadEnded( this, args );
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

        internal void DebugSetUserName( string newUserName)
        {
            // This API is for the sole purpose of allowing unit tests to connect to the service as a valid user so that the channel can be opened.
            // Once the channel is open then the user name can be changed to allow testing scenarios where invalid accesstokens get sent over the wire.
            this.userName = newUserName;
            this.wsAPI.DebugSetUserName(newUserName);
        }

        public WebServiceAPIWrapper WebServiceAPI
        {
            get 
            {
                assertInitialized();
                return wsAPI; 
            }
        }

        public Cache Cache
        {
            get 
            {
                assertInitialized();
                return cache; 
            }
        }

        public void Initialize( string cachePath )
        {
            if ( initialized == true )
            {
                return;
            }
            initialized = true;

            if ( string.IsNullOrEmpty(cachePath) == false )
            {
                cache = new Cache(cachePath);
            }
        }

        public bool Connect(string url, string userName, bool refreshCachedMetaData, out string errorMessage, out bool invalidUserName)
        {
            assertInitialized();
            this.userName = userName;
            this.lastConnectionUrl = url;
            bool succeeded = connectInternal(url, userName, refreshCachedMetaData, out errorMessage, out invalidUserName);
            return succeeded;
        }

        public void Reconnect()
        {
            Disconnect();
            if ( string.IsNullOrEmpty(this.lastConnectionUrl) == false ) 
            {
                string notUsed;
                bool invalidUserName;
                bool connected = connectInternal(lastConnectionUrl, this.userName, false, out notUsed, out invalidUserName);
                if (connected == false)
                {
                    wsAPI = null;
                }
            }
        }

        private bool connectInternal(string url, string userName, bool refreshCachedMetaData, out string errorMessage, out bool invalidUserName)
        {
            if (wsAPI == null)
            {
                wsAPI = new WebServiceAPIWrapper();
                onChannelStateChanged(WebServiceChannelState.Created);
                subscribeChannelEvents();
            }
            bool succeeded = wsAPI.Connect(url, userName, out errorMessage, out invalidUserName);
            if ( succeeded == false )
            {
                return false;
            }

            // conditionally update the cache metadata
            if (refreshCachedMetaData == true)
            {
                NoteTypeInfo[] noteTypes = wsAPI.GetAllNoteTypes();
                this.Cache.MetaData.AssignNoteTypes( noteTypes );

                UserInfo[] users = wsAPI.GetAllUsers();
                this.Cache.MetaData.AssignUsers(users);

                this.Cache.SaveMetaData();
            }

            return true;
        }

        public void Disconnect()
        {
            assertInitialized();
            if ( wsAPI != null )
            {
                try
                {
                    wsAPI.Disconnect();
                }
                catch
                {
                }
                finally
                {
                    unsubscribeChannelEvents();
                    wsAPI = null;
                }
            }
        }

        public bool IsConnected()
        {
            assertInitialized();
            if ( wsAPI == null )
            {
                return false;
            }
            if ( wsAPI.ChannelState == null )
            {
                return false;
            }
            return (wsAPI.ChannelState.Value == WebServiceChannelState.Opened);
        }

        public bool IsNetworkAvailable()
        {
            #if WINDOWS

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    NetworkInterface[] netInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface netInterface in netInterfaces)
                    {
                        if (netInterface.OperationalStatus == OperationalStatus.Up)
                        {
                            if ((netInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel) &&
                                (netInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                            {
                                IPv4InterfaceStatistics statistics = netInterface.GetIPv4Statistics();

                                // empirical testing indicates that once an interface
                                // comes online it has already acquired statistics for
                                // both received and sent bytes, so we use that fact to
                                // define 'availability'
                                if ((statistics.BytesReceived > 0) && (statistics.BytesSent > 0))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            #else 
                // MONOTOUCH
                return Reachability.IsHostReachable("www.google.com");
            #endif
        }

        public WebServiceAPIResult DownLoadOrderToCache( int orderID, out string errorMessage )
        {
            assertInitialized();
            try
            {
                return downLoadOrderToCacheInternal(orderID, out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = buildExceptionErrorMessage(ex);
                return WebServiceAPIResult.Fail;
            }
        }

        public WebServiceAPIResult DownLoadOrdersToCacheByPONumber(string poNumber, bool scheduledOnly, bool activeOnly, int maxOrders, out string errorMessage, out int[] downloadedIDs)
        {
            assertInitialized();
            try
            {
                return downLoadOrdersToCacheByPONumberInternal(poNumber, scheduledOnly, activeOnly, maxOrders, out errorMessage, out downloadedIDs);
            }
            catch (Exception ex)
            {
                errorMessage = buildExceptionErrorMessage(ex);
                downloadedIDs = null;
                return WebServiceAPIResult.Fail;
            }
        }


        public WebServiceAPIResult SynchronizeCachedOrder(CacheOrder cachedOrder, out string errorMessage)
        {
            assertInitialized();
            try
            {
                return synchronizeCachedOrderInternal(cachedOrder, out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = buildExceptionErrorMessage(ex);
                return WebServiceAPIResult.Fail;
            }
        }

		// brute force upload - no checking for what needs to be sent
        public WebServiceAPIResult UploadCachedOrder(CacheOrder cachedOrder, out string errorMessage)
        {
            assertInitialized();
            try
            {
                return uploadCachedOrderInternal(cachedOrder, out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = buildExceptionErrorMessage(ex);
                return WebServiceAPIResult.Fail;
            }
        }
		 
        private WebServiceAPIResult downLoadOrdersToCacheByPONumberInternal(string poNumber, bool scheduledOnly, bool activeOnly, int maxOrders, out string errorMessage, out int[] downloadedIDs)
        {
            errorMessage = null;
            downloadedIDs = null;

            if (IsConnected() == false)
            {
                errorMessage = "Cannot download order while service is disconnected";
                return WebServiceAPIResult.ConnectivityFail;
            }

            downloadedIDs = wsAPI.GetOrdersByPONumber(poNumber, scheduledOnly, activeOnly, maxOrders);
            foreach (int id in downloadedIDs)
            {
                WebServiceAPIResult result = downLoadOrderToCacheInternal(id, out errorMessage);
                if ( result != WebServiceAPIResult.Success )
                {
                    return result;
                }
            }
            return WebServiceAPIResult.Success;
        }

        private WebServiceAPIResult downLoadOrderToCacheInternal(int id, out string errorMessage)
        {
            errorMessage = null;

            if (IsConnected() == false)
            {
                errorMessage = "Cannot download order while service is disconnected";
                return WebServiceAPIResult.ConnectivityFail;
            }

            // get the order details, notes, and photo references
            OrderInfo order = wsAPI.GetOrderByID(id);
            if (order == null)
            {
                errorMessage = string.Format("Could not download order ID={0}", id  );
                return WebServiceAPIResult.Fail;
            }

            // conditionally download the photos if needed and save the images in the cache
            List<PhotoInfo> photosToDownload = new List<PhotoInfo>();
			foreach (PhotoInfo photo in order.Photos)
            {
				// Check here to see if the photo is in the 'NewPhotos' section of the cached order.  If it is then we can just
				// copy the photo from new to previous and avoid the download.  Note that we still needed to get the server's
				// file name
				bool foundMatch = false;
				PhotoInfo[] photosInCache = Cache.GetNewPhotosForOrder( order.ID );
                PhotoInfo matchingCachePhoto = null;
				if ( photosInCache != null )
				{
					foreach ( PhotoInfo cachePhoto in photosInCache )
					{
						if ( cachePhoto.Equals(photo) )
						{
							foundMatch = true;
                            matchingCachePhoto = cachePhoto;
							break;
						}
					}
				}
				if ( foundMatch == true )
				{
                    // modify the cache order by copying the file to its new location and using the server photo info as the updated info
                    string targetDirectory = Cache.GetDownloadedPhotosFolderName(order.ID);
                    string targetFileName = photo.FilePath;
                    string targetFullPath = Path.Combine( targetDirectory, targetFileName);
                    // copy the file to the existing photo area with the proper info from the server
                    try
                    {
                        if (Directory.Exists(targetDirectory) == false)
                        {
                            Directory.CreateDirectory(targetDirectory);
                        }
                        File.Copy(matchingCachePhoto.FilePath, targetFullPath);
                    }
                    catch
                    {
                        photosToDownload.Add(photo);
                        continue;
                    }
				}
				else
				{
	                // check cache first.  we want to see if we have the servers photo in our cache but file name/path
					// cannot be be used. this is because we use a temp path on the client and it is ultimately resolved on the server.
					// so we check the combination of 'entered by' and when it was taken to see if it is a match
					foundMatch = false;
					photosInCache = Cache.GetPhotosForOrder( order.ID );
					if ( photosInCache != null )
					{
						foreach ( PhotoInfo cachePhoto in photosInCache )
						{
							if ( cachePhoto.Equals(photo) )
							{
								foundMatch = true;
								break;
							}
						}
					}
					if ( foundMatch == false )
					{
						photosToDownload.Add( photo );
					}
				}
            }
			
			int numPhotosDownloaded = 0;
			if ( photosToDownload.Count > 0 )
			{
				onPhotosDownloadStarted( photosToDownload.Count);
			}
			foreach (PhotoInfo photo in photosToDownload)
            {
                byte[] photoBytes = wsAPI.DownloadPhoto(photo.ID);
                if (photoBytes != null)
                {
                    this.Cache.SaveDownloadedPhoto(order.ID, photo, photoBytes);
                }
				numPhotosDownloaded++;
				onPhotoDownloaded(numPhotosDownloaded, photosToDownload.Count);
            }
			if ( photosToDownload.Count > 0 )
			{
				onPhotosDownloadEnded( numPhotosDownloaded );
			}
			
            // Download the diagram if needed and save the image in the cache
            if (order.HasDiagram == true)
            {
                byte[] diagramBytes = wsAPI.DownloadDiagram(order.DiagramNumber);
                if (diagramBytes != null)
                {
                    this.Cache.SaveDownloadedDiagram(order.ID, order.DiagramNumber, diagramBytes);
                }
            }

            // now that we have the files copied from the new area to the download area we need to empty out the new photos folder
            Cache.ClearNewPhotoDirectory( order.ID );

			// save the new order
            CacheOrder cacheOrderToSave = new CacheOrder(order, null, null, CacheStatus.Synched);;
            if (this.cache.SaveOrder(cacheOrderToSave) == true)
            {
                return WebServiceAPIResult.Success;
            }
            else
            {
                errorMessage = "Failed to save order to local cache";
                return WebServiceAPIResult.Fail;
            }
        }


        private WebServiceAPIResult synchronizeCachedOrderInternal(CacheOrder cachedOrder, out string errorMessage)
        {
            errorMessage = null;

            if ( cachedOrder.Status != CacheStatus.SynchPending )
            {
                // this is an API usage error
                throw new InvalidOperationException("Cannot synchronize a cached order that has not been set to the synch pending state");
            }

            if (IsConnected() == false)
            {
                errorMessage = "Cannot synchronize order while service is disconnected";
                return WebServiceAPIResult.ConnectivityFail;
            }

			// establish the baseline client order and the server order against which to compare it
            OrderInfo clientOrder = cachedOrder.Order;
            OrderInfo serverOrder = wsAPI.GetOrderByID(clientOrder.ID);
            if (serverOrder == null)
            {
                errorMessage = string.Format("Could not download OrderID={0} PO_Number={1} for syncronization", clientOrder.ID, clientOrder.PONumber );
                return WebServiceAPIResult.Fail;
            }

            // calculate the differences between the local client version of the order and the server version.
            // This is necessary because the server data can be updated out of band to this client end-point

            // get a list of notes that are NEW in the client but not yet on the server.  We do it this way in case previous sync attempts failed
            // part-way through, so instead of just uploading the NEW notes we make sure the server does not already have the note.
            List<NoteInfo> missingServerNotes = getMissingNotes( serverOrder.Notes, cachedOrder.NewNotes );

            // get a list of photos that are NEW in the client but not yet on the server.  We do it this way in case previous sync attempts failed
            // part-way through, so instead of just uploading the NEW photos we make sure the server does not already have the photo.
            List<PhotoInfo> missingServerPhotos = getMissingPhotos(serverOrder.Photos, cachedOrder.NewPhotos);

            // Note: the client does not create diagrams so there will never be a missing server diagram

            // upload the notes that are missing on the server
			onNotesUploadStarted(missingServerNotes.Count);
			int numNotesUploaded = 0;;
			foreach ( NoteInfo note in missingServerNotes )
            {
                if (wsAPI.AddNote(clientOrder.ID, note) == false)
                {
                    errorMessage = string.Format("Failed to upload note to server for order ID={0} PO_Number={1)", clientOrder.ID, clientOrder.PONumber);
                    return WebServiceAPIResult.Fail;
                }
				numNotesUploaded++;
				onNoteUploaded(numNotesUploaded, missingServerNotes.Count);
            }
			onNotesUploadEnded(missingServerNotes.Count);
			
            // upload the photos that are missing on the server
			onPhotosUploadStarted(missingServerPhotos.Count);
			int numPhotosUploaded = 0;;
			foreach ( PhotoInfo photo in missingServerPhotos )
            {
	            if ( uploadPhoto( clientOrder.ID, photo, out errorMessage) == false )
	            {
	                return WebServiceAPIResult.Fail;
	            }
				numPhotosUploaded++;
				onPhotoUploaded(numNotesUploaded, missingServerNotes.Count);
            }
			onPhotosUploadEnded(missingServerNotes.Count);

            // download the item into the cache from the server to account for any server items that may have 
			// been missing from the client (or were different)
            var result = downLoadOrderToCacheInternal(clientOrder.ID, out errorMessage);
            if ( result != WebServiceAPIResult.Success)
            {
                return result;
            }
            CacheOrder updatedClientOrder = this.Cache.GetOrder(clientOrder.ID);
            if ( updatedClientOrder == null )
            {
                errorMessage = string.Format("Failed to download order ID={0} PO_Number={1} for synchronization", clientOrder.ID, clientOrder.PONumber);
                return WebServiceAPIResult.Fail;
            }

            // confirm that the download from the server contains everything it should.
            foreach (NoteInfo note in missingServerNotes)
            {
                if (containsNote(note, updatedClientOrder.Order.Notes) == false)
                {
                    errorMessage = string.Format("Failed to confirm note synch for order ID={0} PO_Number={1} for synchronization", clientOrder.ID, clientOrder.PONumber);
                    return WebServiceAPIResult.Fail;
                }
            }
            foreach (PhotoInfo photo in missingServerPhotos)
            {
                if (containsPhoto(photo, updatedClientOrder.Order.Photos) == false)
                {
                    errorMessage = string.Format("Failed to confirm photo synch for order ID={0} PO_Number={1} for synchronization", clientOrder.ID, clientOrder.PONumber);
                    return WebServiceAPIResult.Fail;
                }
            }

            return WebServiceAPIResult.Success;
        }

        private List<NoteInfo> getMissingNotes(NoteInfo[] baseLineNotes, NoteInfo[] comparisonNotes)
        {
            // return an array of notes that are in the comparison set but missing from the baseline set
            List<NoteInfo> missingNotes = new List<NoteInfo>();
            if ( baseLineNotes == null || comparisonNotes == null )
            {
                return missingNotes;
            }
            foreach ( NoteInfo note in comparisonNotes )
            {
                if ( containsNote( note, baseLineNotes ) == false )
                {
                    missingNotes.Add(note);
                }
            }
            return missingNotes;
        }

        private List<PhotoInfo> getMissingPhotos(PhotoInfo[] baseLinePhotos, PhotoInfo[] comparisonPhotos)
        {
            // return an array of Photos that are in the comparison set but missing from the baseline set
            List<PhotoInfo> missingPhotos = new List<PhotoInfo>();
            if (baseLinePhotos == null || comparisonPhotos == null)
            {
                return missingPhotos;
            }
            foreach (PhotoInfo photo in comparisonPhotos)
            {
                if (containsPhoto(photo, baseLinePhotos) == false)
                {
                    missingPhotos.Add(photo);
                }
            }
            return missingPhotos;
        }

        private bool containsNote(NoteInfo noteToFind, NoteInfo[] notes)
        {
            foreach ( NoteInfo note in notes )
            {
                if ( note.Equals( noteToFind ) == true )
                {
                    return true;
                }
            }
            return false;
        }

        private bool containsPhoto(PhotoInfo photoToFind, PhotoInfo[] photos)
        {
            foreach (PhotoInfo photo in photos)
            {
                if (photo.Equals(photoToFind) == true)
                {
                    return true;
                }
            }
            return false;
        }
		
        private WebServiceAPIResult uploadCachedOrderInternal(CacheOrder cachedOrder, out string errorMessage)
        {
            errorMessage = null;

            if (IsConnected() == false)
            {
                errorMessage = "Cannot upload order while service is disconnected";
                return WebServiceAPIResult.ConnectivityFail;
            }
            
            // Add any new notes
            foreach (NoteInfo note in cachedOrder.NewNotes)
            {
                if (this.wsAPI.AddNote(cachedOrder.Order.ID, note) == false)
                {
                    errorMessage = string.Format("Failed to add a new note to existing order ID == {0}", cachedOrder.Order.ID);
                    return WebServiceAPIResult.Fail;
                }
            }

            // Add any new photos
            foreach (PhotoInfo photo in cachedOrder.NewPhotos)
            {
	            if ( uploadPhoto( cachedOrder.Order.ID, photo, out errorMessage) == false )
	            {
	                return WebServiceAPIResult.Fail;
	            }
            }

            return WebServiceAPIResult.Success;
        }

        private bool uploadPhoto( int orderID, PhotoInfo photo, out string errorMessage)
        {
            errorMessage = "";
            bool failed = false;
            byte[] photoBytes = null;
            try
            {
                photoBytes = System.IO.File.ReadAllBytes(photo.FilePath);
            }
            catch
            {
                errorMessage = string.Format("Failed to read cached photo {0}", photo.FilePath);
				return false;
            }
            
			string extension = System.IO.Path.GetExtension(photo.FilePath).TrimStart('.');
            if (this.wsAPI.AddPhoto(orderID, photo, photoBytes, extension) == false)
            {
                errorMessage = string.Format("Failed to add a new photo to existing order ID == {0}", orderID);
				return false;
            }
            return true;
        }

        private string buildExceptionErrorMessage(Exception ex)
        {
            try
            {
                // wrap in try catch so we don't let logging error bring it all down
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}\r\n\r\n{1}\r\n\r\n", ex.Message, ex.StackTrace);
                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    sb.AppendFormat("[INNER EXCEPTION] {0}\r\n\r\n{1}\r\n\r\n", innerException.Message, innerException.StackTrace);
                    innerException = innerException.InnerException;
                }
                return sb.ToString();
            }
            catch
            {
                return "Unknown Logging Error";
            }
        }

        private void assertInitialized()
        {
            if (initialized == false)
            {
                throw new InvalidOperationException("Client not initialized");
            }
        }

        private void subscribeChannelEvents()
        {
            if ( wsAPI != null )
            {
                wsAPI.ChannelStateChanged += new EventHandler<WebServiceAPIWrapper.WebServiceChannelStateChangedEventArgs>(wsAPI_ChannelStateChanged);
				wsAPI.DownloadStarted += handleDownloadStarted;
				wsAPI.PartDownloaded += handlePartDownloaded;
				wsAPI.DownloadEnded += handleDownloadEnded;
				wsAPI.UploadStarted += handleUploadStarted;
				wsAPI.PartUploaded += handlePartUploaded;
				wsAPI.UploadEnded += handleUploadEnded;
            }
        }

        private void unsubscribeChannelEvents()
        {
            try
            {
                if ( wsAPI != null )
                {
                    wsAPI.ChannelStateChanged -= new EventHandler<WebServiceAPIWrapper.WebServiceChannelStateChangedEventArgs>(wsAPI_ChannelStateChanged);
					wsAPI.DownloadStarted -= handleDownloadStarted;
					wsAPI.PartDownloaded -= handlePartDownloaded;
					wsAPI.DownloadEnded -= handleDownloadEnded;
					wsAPI.UploadStarted -= handleUploadStarted;
					wsAPI.PartUploaded -= handlePartUploaded;
					wsAPI.UploadEnded -= handleUploadEnded;
                }
            }
            catch
            {
            }
        }

		
        void handleUploadEnded (object sender, TransferProgressEventArgs e)
        {
			// bubble up
			onUploadEnded( e.TotalToTransfer );
        }

        void handlePartUploaded (object sender, TransferProgressEventArgs e)
        {
			// bubble up
			onPartUploaded( e.NumTransferred, e.TotalToTransfer );
        }

        void handleUploadStarted (object sender, TransferProgressEventArgs e)
        {
			// bubble up
			onUploadStarted( e.TotalToTransfer );
        }

        void handleDownloadEnded (object sender, TransferProgressEventArgs e)
        {
			// bubble up
			onDownloadEnded( e.TotalToTransfer );
        }

        void handlePartDownloaded (object sender, TransferProgressEventArgs e)
        {
			// bubble up
			onPartDownloaded( e.NumTransferred, e.TotalToTransfer );
        }

        void handleDownloadStarted (object sender, TransferProgressEventArgs e)
        {
			// bubble up
			onDownloadStarted( e.TotalToTransfer );
        }

        void wsAPI_ChannelStateChanged(object sender, WebServiceAPIWrapper.WebServiceChannelStateChangedEventArgs e)
        {
            // bubble up the state change event
            onChannelStateChanged(e.State);
        }

        public void Dispose()
        {
            Disconnect();        
        }

    }

    public class WebServiceNotConnectedException : Exception
    {
        private WebServiceNotConnectedException() { }

        public WebServiceNotConnectedException(string message)
            : base(message)
        {
        }
    }

    public enum WebServiceChannelState
    {
        Created,
        Closed,
        Closing,
        Faulted,
        Opened,
        Opening
    }

    public enum WebServiceAPIResult
    {
        Success,
        Fail,
        ConnectivityFail,
    }


}
