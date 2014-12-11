using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using CFI;
using CFI.Utilities;
using CFIDataAccess.Models;
using System.Linq;

namespace CFIServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class JobInspectionService : IJobInspection
    {
        private static ConcurrentDictionary<string, UserInfo> userNameTable = null;
        
        static JobInspectionService()
        {
            HostInitializer.Initialize();

            try
            {
                userNameTable = buildUserNameTable();
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public JobInspectionService()
        {
            logServiceProcessInfo();
            logClientConnectionInfo();
        }

        public string Echo(string accessToken, string returnThis)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return returnThis;
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string GetAllNoteTypes(string accessToken)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    var query =
                        from noteType in db.NoteTypes
                        select noteType;

                    NoteTypeInfo[] noteTypes = ConversionUtils.ToNoteTypeInfoArray(query.ToList<NoteType>());
                    return NoteTypeInfo.BuildNoteTypesXml(noteTypes);
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string GetAllUsers(string accessToken)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                UserInfo[] users = getAllUsersInternal();
                return UserInfo.BuildUsersXml(users);
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        private static UserInfo[] getAllUsersInternal()
        {
            using (CFIEntities db = new CFIEntities())
            {
                var query =
                    from employee in db.Employees
                    where employee.Active == true
                    select employee;

                UserInfo[] users = ConversionUtils.ToUserInfoArray(query.ToList<Employee>());
                return users;
            }
        }

        public int[] GetOrderIDsByCustomerLastName(string accessToken, string lastName, bool scheduledOnly, bool activeOnly, int maxOrders)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    var query =
                        from o in db.Orders
                        where (o.Customer.LastName == lastName) && ( !scheduledOnly || o.Scheduled ) && ( !activeOnly || ( !o.Cancelled && !o.Deleted && !o.Billed ) )
                        orderby o.Customer.LastName
                        select o.OrderID;

                    return query.Take(maxOrders).ToArray<int>();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public int[] GetOrderIDsByStoreNumber(string accessToken, string storeNumber, bool scheduledOnly, bool activeOnly, int maxOrders)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    var query =
                        from o in db.Orders
                        where (o.Store.StoreNumber == storeNumber) && (!scheduledOnly || o.Scheduled) && (!activeOnly || (!o.Cancelled && !o.Deleted && !o.Billed))
                        orderby o.Customer.LastName
                        select o.OrderID;

                    return query.Take(maxOrders).ToArray<int>();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public int[] GetOrderIDsByDateRange(string accessToken, DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                if (range.Start > range.End)
                {
                    DateTime temp = range.Start;
                    range.Start = range.End;
                    range.End = temp;
                }

                using (CFIEntities db = new CFIEntities())
                {
                    var query =
                        from o in db.Orders
                        where (
                              (o.ScheduleStartDate.Value >= range.Start) && 
                              (o.ScheduleStartDate.Value <= range.End) && 
                              (!scheduledOnly || o.Scheduled) && 
                              (!activeOnly || (!o.Cancelled && !o.Deleted && !o.Billed))
                              )
                        orderby o.ScheduleStartDate
                        select o.OrderID;

                    return query.Take(maxOrders).ToArray<int>();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public int[] GetOrderIDsByMultipleCriteria(string accessToken, string lastName, string poNumber, string storeNumber, DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                if ( range == null )
                {
                    range = new DateRange();
                    range.Start = DateTime.MinValue;
                    range.End = DateTime.MinValue;
                }

                if (range.Start > range.End)
                {
                    DateTime temp = range.Start;
                    range.Start = range.End;
                    range.End = temp;
                }

                // delegate to simpler searches as needed
                bool bothMin = range.Start == DateTime.MinValue && range.End == DateTime.MinValue;
                bool bothMax = range.Start == DateTime.MaxValue && range.End == DateTime.MaxValue;
                bool useDateRange = ( !bothMin && !bothMax);  
                bool usePONumber = ( string.IsNullOrEmpty(poNumber) == false );
                bool useLastName = (string.IsNullOrEmpty(lastName) == false);
                bool useStoreNumber = (string.IsNullOrEmpty(storeNumber) == false);

                if ( useLastName == false && usePONumber == false && useDateRange == false && useStoreNumber == false)
                {
                    return new int[0];
                }
                else if (useLastName == false && usePONumber == false && useDateRange == true && useStoreNumber == false)
                {
                    return GetOrderIDsByDateRange(accessToken, range, scheduledOnly, activeOnly, maxOrders);
                }
                else if (useLastName == true && usePONumber == false && useDateRange == false && useStoreNumber == false)
                {
                    return GetOrderIDsByCustomerLastName(accessToken, lastName, scheduledOnly, activeOnly, maxOrders);
                }
                else if (useLastName == false && usePONumber == false && useDateRange == false && useStoreNumber == true)
                {
                    return GetOrderIDsByStoreNumber(accessToken, storeNumber, scheduledOnly, activeOnly, maxOrders);
                }
                else if (useLastName == false && usePONumber == true && useDateRange == false && useStoreNumber == false)
                {
                    return GetOrdersByPONumber(accessToken, poNumber, scheduledOnly, activeOnly, maxOrders);
                }

                // if we made it to here then it is a multi-criteria search
                using (CFIEntities db = new CFIEntities())
                {
                    var query =
                        from o in db.Orders
                        where ( 
                              ( !usePONumber    || o.PurchaseOrderNumber == poNumber                                                        ) &&
                              ( !useDateRange   || ((o.ScheduleStartDate.Value >= range.Start) && (o.ScheduleStartDate.Value <= range.End)) ) &&
                              ( !useLastName    || o.Customer.LastName == lastName                                                          ) &&
                              ( !useStoreNumber || o.Store.StoreNumber == storeNumber                                                       ) &&
                              ( !scheduledOnly  || o.Scheduled                                                                              ) && 
                              ( !activeOnly     || (!o.Cancelled && !o.Deleted && !o.Billed)                                                )
                              )
                        select o.OrderID;
                    return query.Take(maxOrders).ToArray<int>();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public int[] GetOrderIDs(string accessToken, bool scheduledOnly, bool activeOnly, int maxOrders)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    var query = 
                        from o in db.Orders
                        where (!scheduledOnly || o.Scheduled) && (!activeOnly || (!o.Cancelled && !o.Deleted && !o.Billed))
                        orderby o.ScheduleStartDate
                        select o.OrderID;
                    return query.Take(maxOrders).ToArray<int>();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public OrderInfo GetOrderByID(string accessToken, int ID)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    Order order = getOrderByID(ID, db);
                    if (order == null)
                    {
                        return null;
                    }
                    else
                    {
                        return ConversionUtils.ToOrderInfo(order, db);
                    }
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public int[] GetOrdersByPONumber(string accessToken, string poNumber, bool scheduledOnly, bool activeOnly, int maxOrders)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    var query =
                        from o in db.Orders
                        where (o.PurchaseOrderNumber == poNumber) && (!scheduledOnly || o.Scheduled) && (!activeOnly || (!o.Cancelled && !o.Deleted && !o.Billed))
                        select o.OrderID;
                    return query.Take(maxOrders).ToArray<int>();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public bool AddNote(string accessToken, int orderID, NoteInfo note)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    Order order = getOrderByID(orderID, db);
                    PONote poNote = ConversionUtils.ToPONote(note, orderID);
                    order.PONotes.Add(poNote);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public NoteInfo GetNote(string accessToken, int noteID)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    PONote poNote = getPONoteByID(noteID, db);
                    if (poNote == null)
                    {
                        return null;
                    }
                    else
                    {
                        return ConversionUtils.ToNoteInfo(poNote, db);
                    }
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string GetNotes(string accessToken, int orderID)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
               using (CFIEntities db = new CFIEntities())
                {
                    PONote[] poNotes = getPONotesByOrderID(orderID, db);
                    if (poNotes == null)
                    {
                        return null;
                    }
                    else
                    {
                        NoteInfo[] notes = ConversionUtils.ToNoteInfoArray(poNotes, db);
                        return NoteInfo.BuildNotesXml(notes);
                    }
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public bool DeleteNote(string accessToken, int noteID)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    PONote poNote = getPONoteByID(noteID, db);
                    if (poNote != null)
                    {
                        poNote.Deleted = true;
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public bool AddPhoto(string accessToken, int orderID, PhotoInfo photo, string fileExtension, string uploadedFileClaimToken)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    Order order = getOrderByID(orderID, db);

                    // the file name will depend on the po number and how many other photos may have already been added
                    string fileName = getPhotoFileName(order.OrderID, fileExtension);
                    photo.FilePath = fileName;
                    
                    // claim the uploaded file and name it and store it in the proper location
                    byte[] photoBytes = Globals.FileTransferManager.Uploader.ClaimFile(uploadedFileClaimToken);
                    savePhoto(order.OrderID, fileName, photoBytes);

                    POPhoto poPhoto = ConversionUtils.ToPOPhoto(photo, orderID);
                    order.POPhotos.Add(poPhoto);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public PhotoInfo GetPhoto(string accessToken, int photoID)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    POPhoto poPhoto = getPOPhotoByID(photoID, db);
                    if (poPhoto == null)
                    {
                        return null;
                    }
                    else
                    {
                        return ConversionUtils.ToPhotoInfo(poPhoto, db);
                    }
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string GetPhotos(string accessToken, int orderID)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    POPhoto[] poPhotos = getPOPhotosByOrderID(orderID, db);
                    if (poPhotos == null)
                    {
                        return null;
                    }
                    else
                    {
                        PhotoInfo[] photos = ConversionUtils.ToPhotoInfoArray(poPhotos, db);
                        return PhotoInfo.BuildPhotosXml(photos);
                    }
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public bool DeletePhoto(string accessToken, int photoID)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    POPhoto poPhoto = getPOPhotoByID(photoID, db);
                    if (poPhoto != null)
                    {
                        poPhoto.Deleted = true;
                        db.SaveChanges();
                        deletePhoto(poPhoto.Order.OrderID, poPhoto.FilePath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public string QueueDiagramDownload(string accessToken, string diagramFilename)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    return queueDiagramDownloadInternal(diagramFilename, db);
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }
 

        public string QueuePhotoDownload(string accessToken, int photoID)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    return queuePhotoDownloadInternal(photoID, db);
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string[] GetLogDirectoryNames( string accessToken )
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return LogAPI.GetLogFolderNames();
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string GetCurrentLogDirectoryName(string accessToken )
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return LogAPI.GetCurrentLogDirectoryName();
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string[] GetLogFileNames(string accessToken, string logDirectoryName)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return LogAPI.GetLogFileNames( logDirectoryName );
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string QueueLogFileDownload(string accessToken, string directoryName, string fileName)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return queueLogFileDownloadInternal( directoryName, fileName );
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public string UploadFileStart(string accessToken, int totalBytes, int chunkSize)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return uploadFileStartInternal( totalBytes, chunkSize );
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public bool UploadFilePart(string accessToken, string transferToken, int chunkIndex, byte[] bytes)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return uploadFilePartInternal( transferToken, chunkIndex, bytes );
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public bool UploadFileCancel(string accessToken, string transferToken)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return uploadFileCancelInternal(transferToken);
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public bool UploadFileEnd(string accessToken, string transferToken)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return uploadFileEndInternal(transferToken);
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public int DownloadFileStart(string accessToken, string transferToken, int chunkSize)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return downloadFileStartInternal(transferToken, chunkSize);
            }
            catch (Exception ex)
            {
                logException(ex);
                return 0;
            }
        }

        public byte[] DownloadFilePart(string accessToken, string transferToken, int chunkIndex)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return downloadFilePartInternal(transferToken, chunkIndex);
            }
            catch (Exception ex)
            {
                logException(ex);
                return null;
            }
        }

        public bool DownloadFileCancel(string accessToken, string transferToken)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return downloadFileCancelInternal(transferToken);
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public bool DownloadFileEnd(string accessToken, string transferToken)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                return downloadFileEndInternal(transferToken);
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        public bool UpdateNoteText(string accessToken, int noteID, string newText)
        {
            logMethodInvocation();
            checkAccessToken(accessToken);
            try
            {
                using (CFIEntities db = new CFIEntities())
                {
                    PONote poNote = getPONoteByID(noteID, db);
                    poNote.NoteText = newText;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                logException(ex);
                return false;
            }
        }

        private string uploadFileStartInternal(int totalBytes, int chunkSize)
        {
            return Globals.FileTransferManager.Uploader.StartUpload(totalBytes, chunkSize);
        }

        private bool uploadFilePartInternal(string transferToken, int chunkIndex, byte[] part)
        {
            return Globals.FileTransferManager.Uploader.UploadPart(transferToken, chunkIndex, part);
        }

        private bool uploadFileCancelInternal(string transferToken)
        {
            return Globals.FileTransferManager.Uploader.CancelUpload(transferToken);
        }

        private bool uploadFileEndInternal(string transferToken)
        {
            return Globals.FileTransferManager.Uploader.EndUpload(transferToken);
        }

        private int downloadFileStartInternal(string transferToken, int chunkSize)
        {
            return Globals.FileTransferManager.Downloader.StartDownload( transferToken, chunkSize );
        }

        private byte[] downloadFilePartInternal(string transferToken, int chunkIndex)
        {
            return Globals.FileTransferManager.Downloader.DownloadPart(transferToken, chunkIndex);
        }

        private bool downloadFileCancelInternal(string transferToken)
        {
            return Globals.FileTransferManager.Downloader.CancelDownload(transferToken);
        }

        private bool downloadFileEndInternal(string transferToken)
        {
            return Globals.FileTransferManager.Downloader.EndDownload(transferToken);
        }

        private string getPhotoFileName(int orderID, string fileExtension)
        {
            string targetDir = getTargetPhotoDirectory(orderID);
            List<string> fileNames = new List<string>();


            if ( Directory.Exists(targetDir) == false )
            {
                Directory.CreateDirectory( targetDir );
            }
            foreach (string file in Directory.GetFiles(targetDir))
            {
                fileNames.Add( Path.GetFileName( file ).ToLower() );
            }

            for (int i = 0; i < (fileNames.Count + 1); i++ )
            {
                string candidateFileName = string.Format("photo_{0}.{1}", i+1, fileExtension).ToLower();
                if ( fileNames.Contains( candidateFileName ) == false )
                {
                    return candidateFileName;
                }
            }
            return null;
        }

        private void savePhoto(int orderID, string fileName, byte[] photoBytes)
        {
            string target = Path.Combine( getTargetPhotoDirectory(orderID), fileName);
            try
            {
                if ( File.Exists(target) == true )
                {
                    File.Delete(target);
                }
            }
            catch
            {
            }
            File.WriteAllBytes(target, photoBytes);
        }

        private void deletePhoto(int orderID, string fileName)
        {
            string targetDir = getTargetPhotoDirectory(orderID);
            string targetFile = Path.Combine(targetDir, fileName);
            try
            {
                File.Delete(targetFile);

                // if the containing folder is now empty then delete it as well
                if ( Directory.GetFiles(targetDir).Length == 0 )
                {
                    Directory.Delete( targetDir );
                }
            }
            catch
            {
            }
        }

        private string getTargetPhotoDirectory(int orderID)
        {
            string photoDirectory = getSetting("PhotoFolder");
            return Path.Combine( photoDirectory , string.Format("order_id_{0}", orderID));
        }

        private Order getOrderByID(int orderID, CFIEntities db)
        {
            var query =
                from o in db.Orders
                where (o.OrderID == orderID)
                select o;

            Order[] orders = query.ToArray<Order>();
            if (orders.Length == 0)
            {
                return null;
            }
            else
            {
                return orders[0];
            }
        }

        private PONote getPONoteByID(int poNoteID, CFIEntities db)
        {
            var query =
                from p in db.PONotes
                where p.ID == poNoteID
                select p;

            PONote[] poNotes = query.ToArray<PONote>();
            if (poNotes.Length == 0)
            {
                return null;
            }
            else
            {
                PONote poNote = poNotes[0];
                if (poNote.Deleted == false)
                {
                    return poNote;
                }
                else
                {
                    return null;
                }
            }
        }

        private PONote[] getPONotesByOrderID(int orderID, CFIEntities db)
        {
            var query =
                from o in db.Orders
                where o.OrderID == orderID
                select o.PONotes;

            List<PONote> poNotes = new List<PONote>(query.Single<ICollection<PONote>>());

            if (poNotes.Count == 0)
            {
                return null;
            }
            else
            {
                return poNotes.ToArray();
            }
        }

        private POPhoto getPOPhotoByID(int poPhotoID, CFIEntities db)
        {
            var query =
                from p in db.POPhotos
                where p.ID == poPhotoID
                select p;

            POPhoto[] poPhotos = query.ToArray<POPhoto>();
            if (poPhotos.Length == 0)
            {
                return null;
            }
            else
            {
                POPhoto poPhoto = poPhotos[0];
                if (poPhoto.Deleted == false)
                {
                    return poPhoto;
                }
                else
                {
                    return null;
                }
            }
        }

        private POPhoto[] getPOPhotosByOrderID(int orderID, CFIEntities db)
        {
            var query =
                from o in db.Orders
                where o.OrderID == orderID
                select o.POPhotos;

            List<POPhoto> poPhotos = new List<POPhoto>(query.Single<ICollection<POPhoto>>());

            if (poPhotos.Count == 0)
            {
                return null;
            }
            else
            {
                return poPhotos.ToArray();
            }
        }

        private string queuePhotoDownloadInternal(int photoID, CFIEntities db)
        {
            var query =
                from p in db.POPhotos
                where p.ID == photoID
                select p;

            POPhoto poPhoto = query.Single<POPhoto>();
            string targetDir = getTargetPhotoDirectory(poPhoto.Order.OrderID);
            string fullPath = Path.Combine( targetDir, poPhoto.FilePath );
            if ( File.Exists(fullPath) == false )
            {
                return null;
            }
            byte[] bytes = File.ReadAllBytes( fullPath );

            // queue file into downloader
            return Globals.FileTransferManager.Downloader.QueueFile( bytes );
        }

        private string queueDiagramDownloadInternal(string diagramFilename, CFIEntities db)
        {
            LogAPI.WebServiceLog.DebugFormat("Queueing diagram file {0} for download", diagramFilename);

            if (string.IsNullOrEmpty(diagramFilename) == false)
            {
                byte[] bytes = getDiagramFile(diagramFilename);

                // queue file into downloader
                return Globals.FileTransferManager.Downloader.QueueFile(bytes);
            }
            else
            {
                return null;
            }
        }


        private string queueLogFileDownloadInternal(string logDirectoryName, string logFileName)
        {
            LogAPI.WebServiceLog.DebugFormat("Queueing log file {0}/{1} for download", logDirectoryName, logFileName);
            string directory = Path.Combine( LogAPI.LogRootDirectory, logDirectoryName );
            if ( Directory.Exists( directory ) == false )
            {
                throw new ArgumentException( string.Format("no log directory '{0}' exists", directory));
            }
            string filePath = Path.Combine( directory, logFileName );
            if ( File.Exists(filePath) == false )
            {
                // in case the log file just got created and is not on disk yet, retry after a short wait
                System.Threading.Thread.Sleep(2000);
                if (File.Exists(filePath) == false)
                {
                    throw new ArgumentException(string.Format("no log file '{0}' exists", filePath));
                }
            }

            // fetch the file bytes and queue the downloader
            byte[] bytes = File.ReadAllBytes(filePath);
            return Globals.FileTransferManager.Downloader.QueueFile(bytes);
        }

        private byte[] getDiagramFile(string diagramFilename)
        {
            try
            {
                string filePath = Path.Combine(getDiagramFolder(), diagramFilename);

                if (File.Exists(filePath) == false)
                {
                    LogAPI.WebServiceLog.DebugFormat("Diagram file '{0}' does not exist", filePath);
                    return null;
                }
                return File.ReadAllBytes(filePath);
            }
            catch (System.Exception ex)
            {
                LogAPI.WebServiceLog.DebugFormat("Failed to read diagram bytes due to exception: {0}", ex.Message);
                return null;
            }
        }

        private string getDiagramFolder()
        {
            return getSetting("DrawingsFolder");
        }

        private static string getSetting( string settingName )
        {
            using (CFIEntities db = new CFIEntities())
            {
                var query =
                    from setting in db.Settings
                    where setting.Name == settingName
                    select setting;

                Setting[] settings = query.ToArray<Setting>();
                if (settings.Length == 1)
                {
                    return settings[0].Value;
                }
                else
                {
                    return null;
                }
            }
        }

        private void checkAccessToken(string accessToken)
        {
            string userName = null;
            try
            {
                bool badAccessToken = true;
                if (string.IsNullOrEmpty(accessToken) == false)
                {
                    // crack the token open
                    userName = SecurityUtils.UserNameFromAccessToken(accessToken);
                    if (string.IsNullOrEmpty(userName) == false)
                    {
                        badAccessToken = (isValidUserName(userName) == false);
                    }
                }

                if (badAccessToken == true)
                {
                    try
                    {
                        string methodName = new StackFrame(1).GetMethod().Name;
                        LogAPI.WebServiceLog.InfoFormat("Called method '{0}' with bad access token '{1}'", methodName, accessToken);
                    }
                    catch
                    {
                        LogAPI.WebServiceLog.InfoFormat("Failed to properly log accesstoken failure");
                    }

                    throw new ArgumentException(string.Format("Invalid access token '{0}'  {1} Did not recognize user name {2}", accessToken, SecurityUtils.InvalidUserNameMagicTextToken, userName));
                }
            }
            catch (Exception ex)
            {
                logException(ex);
                throw ex;
            }
        }

        private bool isValidUserName(string userName)
        {
            if ( userNameTable.ContainsKey(userName) == false )
            {
                // retrieve the user list in case anything has been added and add new items to the table as needed
                foreach ( UserInfo user in getAllUsersInternal() )
                {
                    userNameTable.TryAdd(user.UserName, user);
                }
            }

            return userNameTable.ContainsKey(userName);
        }

        private static ConcurrentDictionary<string, UserInfo> buildUserNameTable()
        {
            ConcurrentDictionary<string, UserInfo> table = new ConcurrentDictionary<string, UserInfo>( StringComparer.OrdinalIgnoreCase );

            UserInfo[] users = getAllUsersInternal();
            foreach (UserInfo user in users)
            {
                table[user.UserName] = user;
            }

            return table;
        }

        private void logMethodInvocation()
        {
            try
            {
                string methodName = new StackFrame(1).GetMethod().Name;
                LogAPI.WebServiceLog.InfoFormat("Called method '{0}'", methodName);
            }
            catch
            {
                LogAPI.WebServiceLog.InfoFormat("Failed to properly log method invocation");
            }
        }

        private static void logServiceProcessInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("JobInspectionService object instantiated in:\r\n");
            sb.Append(LogAPI.GetProcessInfo());
            LogAPI.WebServiceLog.Info(sb.ToString());
        }

        private void logClientConnectionInfo()
        {
            LogAPI.WebServiceLog.Info("Client Connected:");
        }

        private static void logException(Exception ex)
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
                LogAPI.WebServiceLog.Error(sb.ToString());
            }
            catch
            {
                LogAPI.WebServiceLog.Error("Detailed exception logging failed");
            }
        }

    }
}
