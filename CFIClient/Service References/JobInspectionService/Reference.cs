﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFI.Client.JobInspectionService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="JobInspectionService.IJobInspection")]
    public interface IJobInspection {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/DownloadFileEnd", ReplyAction="http://tempuri.org/IJobInspection/DownloadFileEndResponse")]
        bool DownloadFileEnd(string accessToken, string transferToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetLogDirectoryNames", ReplyAction="http://tempuri.org/IJobInspection/GetLogDirectoryNamesResponse")]
        string[] GetLogDirectoryNames(string accessToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetCurrentLogDirectoryName", ReplyAction="http://tempuri.org/IJobInspection/GetCurrentLogDirectoryNameResponse")]
        string GetCurrentLogDirectoryName(string accessToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetLogFileNames", ReplyAction="http://tempuri.org/IJobInspection/GetLogFileNamesResponse")]
        string[] GetLogFileNames(string accessToken, string logDirectoryName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/QueueLogFileDownload", ReplyAction="http://tempuri.org/IJobInspection/QueueLogFileDownloadResponse")]
        string QueueLogFileDownload(string accessToken, string directoryName, string fileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/Echo", ReplyAction="http://tempuri.org/IJobInspection/EchoResponse")]
        string Echo(string accessToken, string returnThis);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetOrderIDs", ReplyAction="http://tempuri.org/IJobInspection/GetOrderIDsResponse")]
        int[] GetOrderIDs(string accessToken, bool scheduledOnly, bool activeOnly, int maxOrders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetOrderIDsByCustomerLastName", ReplyAction="http://tempuri.org/IJobInspection/GetOrderIDsByCustomerLastNameResponse")]
        int[] GetOrderIDsByCustomerLastName(string accessToken, string lastName, bool scheduledOnly, bool activeOnly, int maxOrders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetOrderIDsByDateRange", ReplyAction="http://tempuri.org/IJobInspection/GetOrderIDsByDateRangeResponse")]
        int[] GetOrderIDsByDateRange(string accessToken, CFI.DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetOrderIDsByStoreNumber", ReplyAction="http://tempuri.org/IJobInspection/GetOrderIDsByStoreNumberResponse")]
        int[] GetOrderIDsByStoreNumber(string accessToken, string storeNumber, bool scheduledOnly, bool activeOnly, int maxOrders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetOrderIDsByMultipleCriteria", ReplyAction="http://tempuri.org/IJobInspection/GetOrderIDsByMultipleCriteriaResponse")]
        int[] GetOrderIDsByMultipleCriteria(string accessToken, string lastName, string poNumber, string storeNumber, CFI.DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetOrdersByPONumber", ReplyAction="http://tempuri.org/IJobInspection/GetOrdersByPONumberResponse")]
        int[] GetOrdersByPONumber(string accessToken, string poNumber, bool scheduledOnly, bool activeOnly, int maxOrders);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetOrderByID", ReplyAction="http://tempuri.org/IJobInspection/GetOrderByIDResponse")]
        CFI.OrderInfo GetOrderByID(string accessToken, int ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetAllNoteTypes", ReplyAction="http://tempuri.org/IJobInspection/GetAllNoteTypesResponse")]
        string GetAllNoteTypes(string accessToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/AddNote", ReplyAction="http://tempuri.org/IJobInspection/AddNoteResponse")]
        bool AddNote(string accessToken, int orderID, CFI.NoteInfo note);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetNote", ReplyAction="http://tempuri.org/IJobInspection/GetNoteResponse")]
        CFI.NoteInfo GetNote(string accessToken, int noteID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetNotes", ReplyAction="http://tempuri.org/IJobInspection/GetNotesResponse")]
        string GetNotes(string accessToken, int orderID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/UpdateNoteText", ReplyAction="http://tempuri.org/IJobInspection/UpdateNoteTextResponse")]
        bool UpdateNoteText(string accessToken, int noteID, string newText);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/DeleteNote", ReplyAction="http://tempuri.org/IJobInspection/DeleteNoteResponse")]
        bool DeleteNote(string accessToken, int noteID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetAllUsers", ReplyAction="http://tempuri.org/IJobInspection/GetAllUsersResponse")]
        string GetAllUsers(string accessToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/AddPhoto", ReplyAction="http://tempuri.org/IJobInspection/AddPhotoResponse")]
        bool AddPhoto(string accessToken, int orderID, CFI.PhotoInfo photo, string fileExtension, string uploadedFileClaimToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/QueuePhotoDownload", ReplyAction="http://tempuri.org/IJobInspection/QueuePhotoDownloadResponse")]
        string QueuePhotoDownload(string accessToken, int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetPhoto", ReplyAction="http://tempuri.org/IJobInspection/GetPhotoResponse")]
        CFI.PhotoInfo GetPhoto(string accessToken, int photoID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/GetPhotos", ReplyAction="http://tempuri.org/IJobInspection/GetPhotosResponse")]
        string GetPhotos(string accessToken, int orderID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/DeletePhoto", ReplyAction="http://tempuri.org/IJobInspection/DeletePhotoResponse")]
        bool DeletePhoto(string accessToken, int photoID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/QueueDiagramDownload", ReplyAction="http://tempuri.org/IJobInspection/QueueDiagramDownloadResponse")]
        string QueueDiagramDownload(string accessToken, string diagramNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/UploadFileStart", ReplyAction="http://tempuri.org/IJobInspection/UploadFileStartResponse")]
        string UploadFileStart(string accessToken, int totalBytes, int chunkSize);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/UploadFilePart", ReplyAction="http://tempuri.org/IJobInspection/UploadFilePartResponse")]
        bool UploadFilePart(string accessToken, string transferToken, int chunkIndex, byte[] bytes);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/UploadFileCancel", ReplyAction="http://tempuri.org/IJobInspection/UploadFileCancelResponse")]
        bool UploadFileCancel(string accessToken, string transferToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/UploadFileEnd", ReplyAction="http://tempuri.org/IJobInspection/UploadFileEndResponse")]
        bool UploadFileEnd(string accessToken, string transferToken);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/DownloadFileStart", ReplyAction="http://tempuri.org/IJobInspection/DownloadFileStartResponse")]
        int DownloadFileStart(string accessToken, string transferToken, int chunkSize);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/DownloadFilePart", ReplyAction="http://tempuri.org/IJobInspection/DownloadFilePartResponse")]
        byte[] DownloadFilePart(string accessToken, string transferToken, int chunkIndex);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobInspection/DownloadFileCancel", ReplyAction="http://tempuri.org/IJobInspection/DownloadFileCancelResponse")]
        bool DownloadFileCancel(string accessToken, string transferToken);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJobInspectionChannel : CFI.Client.JobInspectionService.IJobInspection, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JobInspectionClient : System.ServiceModel.ClientBase<CFI.Client.JobInspectionService.IJobInspection>, CFI.Client.JobInspectionService.IJobInspection {
        
        public JobInspectionClient() {
        }
        
        public JobInspectionClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JobInspectionClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobInspectionClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobInspectionClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool DownloadFileEnd(string accessToken, string transferToken) {
            return base.Channel.DownloadFileEnd(accessToken, transferToken);
        }
        
        public string[] GetLogDirectoryNames(string accessToken) {
            return base.Channel.GetLogDirectoryNames(accessToken);
        }
        
        public string GetCurrentLogDirectoryName(string accessToken) {
            return base.Channel.GetCurrentLogDirectoryName(accessToken);
        }
        
        public string[] GetLogFileNames(string accessToken, string logDirectoryName) {
            return base.Channel.GetLogFileNames(accessToken, logDirectoryName);
        }
        
        public string QueueLogFileDownload(string accessToken, string directoryName, string fileName) {
            return base.Channel.QueueLogFileDownload(accessToken, directoryName, fileName);
        }
        
        public string Echo(string accessToken, string returnThis) {
            return base.Channel.Echo(accessToken, returnThis);
        }
        
        public int[] GetOrderIDs(string accessToken, bool scheduledOnly, bool activeOnly, int maxOrders) {
            return base.Channel.GetOrderIDs(accessToken, scheduledOnly, activeOnly, maxOrders);
        }
        
        public int[] GetOrderIDsByCustomerLastName(string accessToken, string lastName, bool scheduledOnly, bool activeOnly, int maxOrders) {
            return base.Channel.GetOrderIDsByCustomerLastName(accessToken, lastName, scheduledOnly, activeOnly, maxOrders);
        }
        
        public int[] GetOrderIDsByDateRange(string accessToken, CFI.DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders) {
            return base.Channel.GetOrderIDsByDateRange(accessToken, range, scheduledOnly, activeOnly, maxOrders);
        }
        
        public int[] GetOrderIDsByStoreNumber(string accessToken, string storeNumber, bool scheduledOnly, bool activeOnly, int maxOrders) {
            return base.Channel.GetOrderIDsByStoreNumber(accessToken, storeNumber, scheduledOnly, activeOnly, maxOrders);
        }
        
        public int[] GetOrderIDsByMultipleCriteria(string accessToken, string lastName, string poNumber, string storeNumber, CFI.DateRange range, bool scheduledOnly, bool activeOnly, int maxOrders) {
            return base.Channel.GetOrderIDsByMultipleCriteria(accessToken, lastName, poNumber, storeNumber, range, scheduledOnly, activeOnly, maxOrders);
        }
        
        public int[] GetOrdersByPONumber(string accessToken, string poNumber, bool scheduledOnly, bool activeOnly, int maxOrders) {
            return base.Channel.GetOrdersByPONumber(accessToken, poNumber, scheduledOnly, activeOnly, maxOrders);
        }
        
        public CFI.OrderInfo GetOrderByID(string accessToken, int ID) {
            return base.Channel.GetOrderByID(accessToken, ID);
        }
        
        public string GetAllNoteTypes(string accessToken) {
            return base.Channel.GetAllNoteTypes(accessToken);
        }
        
        public bool AddNote(string accessToken, int orderID, CFI.NoteInfo note) {
            return base.Channel.AddNote(accessToken, orderID, note);
        }
        
        public CFI.NoteInfo GetNote(string accessToken, int noteID) {
            return base.Channel.GetNote(accessToken, noteID);
        }
        
        public string GetNotes(string accessToken, int orderID) {
            return base.Channel.GetNotes(accessToken, orderID);
        }
        
        public bool UpdateNoteText(string accessToken, int noteID, string newText) {
            return base.Channel.UpdateNoteText(accessToken, noteID, newText);
        }
        
        public bool DeleteNote(string accessToken, int noteID) {
            return base.Channel.DeleteNote(accessToken, noteID);
        }
        
        public string GetAllUsers(string accessToken) {
            return base.Channel.GetAllUsers(accessToken);
        }
        
        public bool AddPhoto(string accessToken, int orderID, CFI.PhotoInfo photo, string fileExtension, string uploadedFileClaimToken) {
            return base.Channel.AddPhoto(accessToken, orderID, photo, fileExtension, uploadedFileClaimToken);
        }
        
        public string QueuePhotoDownload(string accessToken, int id) {
            return base.Channel.QueuePhotoDownload(accessToken, id);
        }
        
        public CFI.PhotoInfo GetPhoto(string accessToken, int photoID) {
            return base.Channel.GetPhoto(accessToken, photoID);
        }
        
        public string GetPhotos(string accessToken, int orderID) {
            return base.Channel.GetPhotos(accessToken, orderID);
        }
        
        public bool DeletePhoto(string accessToken, int photoID) {
            return base.Channel.DeletePhoto(accessToken, photoID);
        }
        
        public string QueueDiagramDownload(string accessToken, string diagramNumber) {
            return base.Channel.QueueDiagramDownload(accessToken, diagramNumber);
        }
        
        public string UploadFileStart(string accessToken, int totalBytes, int chunkSize) {
            return base.Channel.UploadFileStart(accessToken, totalBytes, chunkSize);
        }
        
        public bool UploadFilePart(string accessToken, string transferToken, int chunkIndex, byte[] bytes) {
            return base.Channel.UploadFilePart(accessToken, transferToken, chunkIndex, bytes);
        }
        
        public bool UploadFileCancel(string accessToken, string transferToken) {
            return base.Channel.UploadFileCancel(accessToken, transferToken);
        }
        
        public bool UploadFileEnd(string accessToken, string transferToken) {
            return base.Channel.UploadFileEnd(accessToken, transferToken);
        }
        
        public int DownloadFileStart(string accessToken, string transferToken, int chunkSize) {
            return base.Channel.DownloadFileStart(accessToken, transferToken, chunkSize);
        }
        
        public byte[] DownloadFilePart(string accessToken, string transferToken, int chunkIndex) {
            return base.Channel.DownloadFilePart(accessToken, transferToken, chunkIndex);
        }
        
        public bool DownloadFileCancel(string accessToken, string transferToken) {
            return base.Channel.DownloadFileCancel(accessToken, transferToken);
        }
    }
}