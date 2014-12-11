using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace CFI.Client
{
	public class Cache
	{
		private const string orderFilePrefix = "order_id_";
		private CacheMetaData metaData = null;

		private string rootDirectory;
		private string ordersDirectory;
		private string diagramsDirectory;
		private string photosDirectory;
		private string metaDataDirectory;


		private Cache() { }

		public Cache(string directoryPath)
		{
			// create the paths
			this.rootDirectory = directoryPath;
			createCacheFolders();

			if (this.rootDirectory == null)
			{
				throw new ArgumentException(string.Format("Cache directory '{0}' does not exist and cannot be created", directoryPath));
			}
		}

		public string RootDirectory
		{
			get { return rootDirectory; }
		}

		public string OrdersDirectory
		{
			get { return ordersDirectory; }
		}

		public string DiagramsDirectory
		{
			get { return diagramsDirectory; }
		}

		public string PhotosDirectory
		{
			get { return photosDirectory; }
		}

		public string MetaDataDirectory
		{
			get { return metaDataDirectory; }
		}

		private void createCacheFolders()
		{
			this.rootDirectory = CreateDirectoryIfNeeded(this.rootDirectory);
			this.ordersDirectory = CreateDirectoryIfNeeded(Path.Combine(this.rootDirectory, "Orders"));
			this.diagramsDirectory = CreateDirectoryIfNeeded(Path.Combine(this.rootDirectory, "Diagrams"));
			this.photosDirectory = CreateDirectoryIfNeeded(Path.Combine(this.rootDirectory, "Photos"));
			this.metaDataDirectory = CreateDirectoryIfNeeded(Path.Combine(this.rootDirectory, "MetaData"));
		}

		public CacheMetaData MetaData
		{
			get
			{
				if ( metaData == null )
				{
					// check for the xml file
					string file = MetaDataFile;
					if (File.Exists(file) == true)
					{
						// load from the file and parse it
						string xml = File.ReadAllText(file);
						metaData = CacheMetaData.ParseXml(xml);
					}

					// meta data can still be null if the file existed but was corrupt and failed to be parsed,
					// so we have to check again
					if ( metaData == null )
					{
						// create default metaData
						metaData = createDefaultMetaData();

						// write to the file for next time
						SaveMetaData();
					}
				}
				return metaData;
			}
		}

		public void SaveMetaData()
		{
			string file = MetaDataFile;
			File.WriteAllText(file, metaData.ToXml());
		}

		public string MetaDataFile
		{
			get
			{
				return Path.Combine(MetaDataDirectory, "metaData.xml");        
			}
		}

		private CacheMetaData createDefaultMetaData()
		{
			// this data can change over time in the DB but we need initial data
			// so we use this snapshot which is valid as of July 23, 2011
			CacheMetaData metaData = new CacheMetaData();
			metaData.AddNoteType( 1, "INTERNAL" );
			metaData.AddNoteType( 2, "NOTE TO EXPEDITOR" );
			metaData.AddNoteType( 3, "NOTE FROM EXPEDITOR" );
			metaData.AddNoteType( 4, "CALL TO CUSTOMER HOME" );
			metaData.AddNoteType( 5, "CALL TO CUSTOMER WORK" );
			metaData.AddNoteType( 6, "CALL TO CUSTOMER ALL NOS." );
			metaData.AddNoteType( 7, "CALL FROM CUSTOMER" );
			metaData.AddNoteType( 8, "CALL TO STORE" );
			metaData.AddNoteType( 9, "CALL FROM STORE" );
			metaData.AddNoteType( 10, "FAX TO STORE" );
			metaData.AddNoteType( 11, "FAX FROM STORE" );
			metaData.AddNoteType( 12, "INFORMATION ONLY" );
			metaData.AddNoteType( 13, "CALL TO INSTALLER" );
			metaData.AddNoteType( 14, "CALL FROM INSTALLER" );
			metaData.AddNoteType( 15, "MANAGER ACTION" );
			metaData.AddNoteType( 16, "CALL TO CUSTOMER MOBILE" );

			metaData.AddUser( 3, "hillary" );
			metaData.AddUser( 6, "dbartram" );
			metaData.AddUser( 7, "kendra2" );
			metaData.AddUser( 9, "rosie" );
			metaData.AddUser( 10, "monica" );
			metaData.AddUser( 11, "michael" );
			metaData.AddUser( 12, "carri" );
			metaData.AddUser( 14, "paul" );
			metaData.AddUser( 20, "administrator" );
			metaData.AddUser( 23, "mbone" );
			metaData.AddUser( 38, "cpace" );
			metaData.AddUser( 39, "abaker" );
			metaData.AddUser( 41, "cbarker" );
			metaData.AddUser( 50, "twalker" );
			metaData.AddUser( 57, "chendrix" );
			metaData.AddUser( 58, "cparris" );
			metaData.AddUser( 59, "thazel" );
			metaData.AddUser( 67, "cmckee" );
			metaData.AddUser( 69, "jthai" );
			metaData.AddUser( 70, "dprice" );
			metaData.AddUser( 72, "bdunn" );
			metaData.AddUser( 75, "athai" );
			metaData.AddUser(76, "ahansen");
			metaData.AddUser(80, "vesparza");
			metaData.AddUser( 81, "tweaver" );
			metaData.AddUser( 84, "lwalker" );
			metaData.AddUser(85, "lbrown");
			metaData.AddUser(86, "cyoung");
			metaData.AddUser(88, "ntodaro");
			metaData.AddUser(89, "bcataneo");
			metaData.AddUser(90, "rstewart");

			return metaData;
		}

		public void Clear()
		{
			if ( Directory.Exists(this.rootDirectory) == true )
			{
				deleteDirectory(this.rootDirectory);
			}
			createCacheFolders();
			this.metaData = null;
		}

        public void ClearNewPhotoDirectory(int orderID)
        {
            string dir = this.GetNewPhotosFolderName(orderID);
            if ( dir != null )
            {
                try
                {
                    deleteDirectory(dir);
                }
                catch
                {
                }
            }
        }

		public bool IsEmpty
		{
			get
			{
				return (Directory.GetFiles(this.ordersDirectory).Length == 0);
			}
		}

		public CacheOrder GetOrder(int orderID)
		{
			try
			{
				List<string> orderFiles = getOrderFiles();
				foreach (string file in orderFiles)
				{
					if (file.Contains(orderID.ToString()) == true)
					{
						if (orderID.ToString() == Path.GetFileNameWithoutExtension(file).Replace(orderFilePrefix, ""))
						{
							string xml = File.ReadAllText(file);
							return CacheOrder.ParseXml(xml);
						}
					}
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		public bool SaveOrder(CacheOrder orderToSave)
		{
			try
			{
				// get the cache file name that we will be working with
				string orderFile = GetOrderFilePath(orderToSave.Order.ID);

				// re-order the indexes on the new notes so they go from [0 .. N-1]
				int index = 0;
				foreach (NoteInfo newNote in orderToSave.NewNotes)
				{
					newNote.ID = index;
					index++;
				}

				// re-order the indexes on the new photos so they go from [0 .. N-1]
				index = 0;
				foreach (PhotoInfo newPhoto in orderToSave.NewPhotos)
				{
					newPhoto.ID = index;
					index++;
				}

				string orderXml = orderToSave.ToXml();
				if (File.Exists(orderFile) == true)
				{
					try
					{
						File.Delete(orderFile);
					}
					catch
					{
					}
				}
				File.WriteAllText(orderFile, orderXml);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void DeleteOrder(int orderID)
		{
			CacheOrder cachedOrder = GetOrder(orderID);
			if (cachedOrder != null)
			{
				// Delete photo files (downloaded and new ones)
				deleteDirectory(GetOrderSpecificPhotosFolderName(orderID));

				// Delete diagram file(s)
				deleteDirectory(GetDownloadedDiagramsFolderName(orderID));

				// Delete the order file
				string path = GetOrderFilePath( orderID );
				bestEffortDeleteFile(path);
			}
		}

		public CacheOrder[] GetAllOrders()
		{
			List<CacheOrder> cachedOrders = new List<CacheOrder>();
			foreach (string file in getOrderFiles())
			{
				// wrap in try/catch in case file is corrupted
				try
				{
					string xml = File.ReadAllText(file);
					CacheOrder cacheOrder = CacheOrder.ParseXml( xml );
					if ( cacheOrder == null)
					{
						// assume corrupted file and delete	
						try
						{
							File.Delete(file);
						}
						catch
						{
						}
					}
					else
					{
						cachedOrders.Add(cacheOrder);
					}
				}
				catch
				{
					try
					{
						File.Delete(file);
					}
					catch
					{
					}
				}
			}
			return cachedOrders.ToArray();
		}

		public CacheOrder[] GetAllDirtyOrders()
		{
			CacheOrder[] orders = GetAllOrders();
			List<CacheOrder> dirtyOrders = new List<CacheOrder>();
			foreach ( CacheOrder order in orders )
			{
				if ( order.Status == CacheStatus.Dirty )
				{
					dirtyOrders.Add(order);
				}
			}
			return dirtyOrders.ToArray();
		}

		public CacheStatus GetOrderStatus(int orderID)
		{
			string file = GetOrderFilePath(orderID);
			XmlDocument doc = new XmlDocument();
			doc.Load(file);
			XmlNode node = doc.SelectSingleNode("./Order/Status");
			return (CacheStatus)Enum.Parse(typeof(CacheStatus), node.InnerText);
		}

		public static string CreateDirectoryIfNeeded(string directoryPath)
		{
			if (Directory.Exists(directoryPath) == true)
			{
				return directoryPath;
			}
			else
			{
				try
				{
					Directory.CreateDirectory(directoryPath);
					if (Directory.Exists(directoryPath) == false)
					{
						return null;
					}
					return directoryPath;
				}
				catch
				{
					return null;
				}
			}
		}

		public bool ContainsOrder(int orderID)
		{
			return GetCachedOrderIDs().Contains(orderID);
		}

		public List<int> GetCachedOrderIDs()
		{
			List<int> ids = new List<int>();
			foreach (string file in Directory.GetFiles(this.ordersDirectory))
			{
				int id = getOrderIDFromFileName(file);
                if ( id != -1 )
                {
                    ids.Add(id);
                }
			}
			return ids;
		}

		public string GetOrderFilePath(int orderID)
		{
			return Path.Combine( this.ordersDirectory, string.Format("{0}{1}.xml", orderFilePrefix, orderID) );
		}

		public static string GetOrderIDFolderName( int orderID )
		{
			return string.Format("OrderID_{0}", orderID);
		}

		public string GetDownloadedPhotosFolderName( int orderID )
		{
			return Path.Combine(GetOrderSpecificPhotosFolderName(orderID), "Downloaded");
		}

		public string GetNewPhotosFolderName(int orderID)
		{
			return Path.Combine(GetOrderSpecificPhotosFolderName(orderID), "New");
		}

		public string GetOrderSpecificPhotosFolderName(int orderID)
		{
			return Path.Combine(this.photosDirectory, GetOrderIDFolderName(orderID));
		}

		public string GetDownloadedDiagramsFolderName(int orderID)
		{
			return Path.Combine(this.diagramsDirectory, GetOrderIDFolderName(orderID));
		}

		public string SaveDownloadedPhoto(int orderID, PhotoInfo photo, byte[] photoBytes)
		{
			string targetDirectory = GetDownloadedPhotosFolderName( orderID );
			CreateDirectoryIfNeeded(targetDirectory);

			string fileName = Path.GetFileName( photo.FilePath );
			string fullPath = Path.Combine( targetDirectory, fileName );

			if ( File.Exists( fullPath ) == true )
			{
				bestEffortDeleteFile( fullPath );
			}
			File.WriteAllBytes(fullPath, photoBytes);
			return fullPath;
		}

		public string GetPhotoPath( int orderID, PhotoInfo photo )
		{
			string targetDirectory = GetDownloadedPhotosFolderName(orderID);
			string fileName = Path.GetFileName( photo.FilePath );
			return Path.Combine( targetDirectory, fileName );
		}
		
		public PhotoInfo[] GetNewPhotosForOrder( int orderID )
		{
			CacheOrder order = GetOrder(orderID);
			if ( order == null )
			{
				return null;
			}
			return order.NewPhotos;
		}
		
		public PhotoInfo[] GetPhotosForOrder( int orderID )
		{
			CacheOrder order = GetOrder(orderID);
			if ( order == null )
			{
				return null;
			}
			return order.Order.Photos;
		}
		
		
		public string SaveDownloadedDiagram(int orderID, string diagramNumber, byte[] diagramBytes)
		{
			string targetDirectory = GetDownloadedDiagramsFolderName(orderID);
			CreateDirectoryIfNeeded(targetDirectory);

			string fullPath = GetDiagramPath( orderID, diagramNumber );
			
			if (File.Exists(fullPath) == true)
			{
				bestEffortDeleteFile(fullPath);
			}
			File.WriteAllBytes(fullPath, diagramBytes);
			return fullPath;
		}
		
		public string GetDiagramPath( int orderID, string diagramNumber )
		{
			string targetDirectory = GetDownloadedDiagramsFolderName(orderID);
			string fileName = string.Format("{0}.pdf",diagramNumber);
			return Path.Combine(targetDirectory, fileName);
		}
		
		public string SaveNewPhoto(int orderID, byte[] photoBytes, string extension)
		{
			string targetDirectory = GetNewPhotosFolderName(orderID);
			CreateDirectoryIfNeeded(targetDirectory);

			string fileName = string.Format("{0}.{1}", Guid.NewGuid().ToString().Replace("-", "").ToUpper(), extension );
			string fullPath = Path.Combine(targetDirectory, fileName);

			if (File.Exists(fullPath) == true)
			{
				bestEffortDeleteFile(fullPath);
			}
			File.WriteAllBytes(fullPath, photoBytes);
			return fullPath;
		}

		private void deleteDirectory(string directory)
		{
			if ((string.IsNullOrEmpty(directory) == true) || (Directory.Exists(directory) == false))
			{
				return;
			}

			// delete child files
			foreach (string file in Directory.GetFiles(directory))
			{
				try
				{
					File.Delete(file);
				}
				catch
				{
				}
			}

			// delete child dirs
			foreach (string childDirectory in Directory.GetDirectories(directory))
			{
				deleteDirectory(childDirectory);
			}

			// delete self
			try
			{
				Directory.Delete(directory);
			}
			catch
			{
			}
		}

		private void bestEffortDeleteFile(string path)
		{
			try
			{
				if (File.Exists(path) == true)
				{
					File.Delete(path);
				}
			}
			catch
			{
			}
		}

		private int getOrderIDFromFileName(string file)
		{
			// strip off the path
			file = Path.GetFileName(file);

			if (file.ToLower().StartsWith(orderFilePrefix) == false)
			{
				return -1;
			}
			int startIndex = orderFilePrefix.Length;
			int dotIndex = file.LastIndexOf(".");
			int length = dotIndex - startIndex;
			return int.Parse( file.Substring( startIndex, length ) );
		}

		private CacheOrder getCachedOrder(string existingOrderFile)
		{
			try
			{
				string xml = File.ReadAllText(existingOrderFile);
				return CacheOrder.ParseXml(xml);
			}
			catch
			{
				return null;
			}
		}

		private List<string> getOrderFiles()
		{
			List<string> filePaths = new List<string>();
			foreach ( string filePath in Directory.GetFiles( this.ordersDirectory ) )
			{
				string fileName = Path.GetFileName(filePath);
				if (fileName.ToLower().StartsWith(orderFilePrefix) == true)
				{
					filePaths.Add(filePath);
				}
			}
			return filePaths;
		}


    }


}
