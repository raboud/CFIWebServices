using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using CFI;
 
namespace CFIServices
{
    [ServiceContract]
    public interface IJobInspection
    {
        // ****************************************************************************
        // * Miscellaneous APIs                                                       *
        // ****************************************************************************

        [OperationContract]
        string Echo(string accessToken, string returnThis);

        // ****************************************************************************
        // * Order APIs                                                               *
        // ****************************************************************************

        [OperationContract]
        int[] GetOrderIDs(string accessToken, bool scheduledOnly, bool activeOnly, int maxOrders);

        [OperationContract]
        int[] GetOrderIDsByCustomerLastName(string accessToken, string lastName, bool scheduledOnly, bool activeOnly, int maxOrders);

        [OperationContract]
        int[] GetOrderIDsByDateRange(string accessToken, DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders);

        [OperationContract]
        int[] GetOrderIDsByStoreNumber(string accessToken, string storeNumber, bool scheduledOnly, bool activeOnly, int maxOrders);

        [OperationContract]
        int[] GetOrderIDsByMultipleCriteria(string accessToken, string lastName, string poNumber, string storeNumber, DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders);

        [OperationContract]
        int[] GetOrdersByPONumber(string accessToken, string poNumber, bool scheduledOnly, bool activeOnly, int maxOrders);

        [OperationContract]
        OrderInfo GetOrderByID(string accessToken, int ID);

        // ****************************************************************************
        // * Note APIs                                                                *
        // ****************************************************************************

        [OperationContract]
        string GetAllNoteTypes(string accessToken);

        [OperationContract]
        bool AddNote(string accessToken, int orderID, NoteInfo note);

        [OperationContract]
        NoteInfo GetNote(string accessToken, int noteID);

        [OperationContract]
        string GetNotes(string accessToken, int orderID);

        [OperationContract]
        bool UpdateNoteText(string accessToken, int noteID, string newText);

        [OperationContract]
        bool DeleteNote(string accessToken, int noteID);

        // ****************************************************************************
        // * User APIs                                                                *
        // ****************************************************************************

        [OperationContract]
        string GetAllUsers(string accessToken);

        // ****************************************************************************
        // * Photo APIs                                                               *
        // ****************************************************************************

        [OperationContract]
        bool AddPhoto(string accessToken, int orderID, PhotoInfo photo, string fileExtension, string uploadedFileClaimToken);

        // the retval is a claim token for downloading the file with a later API call
        [OperationContract]
        string QueuePhotoDownload(string accessToken, int id);

        [OperationContract]
        PhotoInfo GetPhoto(string accessToken, int photoID);

        [OperationContract]
        string GetPhotos(string accessToken, int orderID);

        [OperationContract]
        bool DeletePhoto(string accessToken, int photoID);


        // ****************************************************************************
        // * diagram APIs                                                             *
        // ****************************************************************************

        // the retval is a claim token for downloading the file with a later API call
        [OperationContract]
        string QueueDiagramDownload(string accessToken, string diagramNumber);

        // ****************************************************************************
        // * file upload APIs                                                         *
        // ****************************************************************************

        // returns a transferToken used in other uploading APIs
        [OperationContract]
        string UploadFileStart(string accessToken, int totalBytes, int chunkSize);

        [OperationContract]
        bool UploadFilePart(string accessToken, string transferToken, int chunkIndex, byte[] bytes);

        [OperationContract]
        bool UploadFileCancel(string accessToken, string transferToken);

        [OperationContract]
        bool UploadFileEnd(string accessToken, string transferToken);

        // returns a transferToken used in other downloading APIs
        [OperationContract]
        int DownloadFileStart(string accessToken, string transferToken, int chunkSize);

        [OperationContract]
        byte[] DownloadFilePart(string accessToken, string transferToken, int chunkIndex);

        [OperationContract]
        bool DownloadFileCancel(string accessToken, string transferToken);

        [OperationContract]
        bool DownloadFileEnd(string accessToken, string transferToken);

        // ****************************************************************************
        // * Debugging APIs                                                           *
        // ****************************************************************************

        [OperationContract]
        string[] GetLogDirectoryNames(string accessToken );

        [OperationContract]
        string GetCurrentLogDirectoryName(string accessToken );

        [OperationContract]
        string[] GetLogFileNames( string accessToken, string logDirectoryName );

        [OperationContract]
        string QueueLogFileDownload(string accessToken, string directoryName, string fileName);





    }


}
