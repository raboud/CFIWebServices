using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CFI;
using CFIDataAccess.Models;
using CFI.Utilities;

namespace CFIServices
{
    public static class ConversionUtils
    {
        private class CustomerInfoSorter : IComparer<CustomerInfo>
        {
            public int Compare(CustomerInfo x, CustomerInfo y)
            {
                return buildComparisonText(x).CompareTo(buildComparisonText(y));
            }

            private string buildComparisonText(CustomerInfo info)
            {
                string retVal;
                if ((string.IsNullOrEmpty(info.LastName) == false) &&
                    (string.IsNullOrEmpty(info.FirstName) == false))
                {
                    retVal = string.Format("{0}, {1}", info.LastName, info.FirstName);
                }
                else if (string.IsNullOrEmpty(info.FirstName))
                {
                    retVal = info.LastName;
                }
                else
                {
                    retVal = info.FirstName;
                }
                return retVal;
            }
        }

        public static CustomerInfo[] ToCustomerInfoArray( IEnumerable<Customer> entityCustomers, bool sort )
        {
            List<CustomerInfo> customers = new List<CustomerInfo>();
            foreach ( Customer entityCustomer in entityCustomers )
            {
                customers.Add( ToCustomerInfo( entityCustomer ) );
            }

            if ( sort == true )
            {
                customers.Sort( new CustomerInfoSorter() );
            }

            return customers.ToArray();
        }

        public static CustomerInfo ToCustomerInfo(Customer entityCustomer)
        {
            CustomerInfo customer = new CustomerInfo();
            customer.ID = entityCustomer.CustomerID;
            customer.LastName = entityCustomer.LastName;
            customer.FirstName = entityCustomer.FirstName;
            return customer;
        }

        public static OrderInfo[] ToOrderInfoArray(IEnumerable<Order> entityOrders, CFIEntities db)
        {
            List<OrderInfo> orders = new List<OrderInfo>();
            foreach (Order entityOrder in entityOrders)
            {
                orders.Add(ToOrderInfo(entityOrder, db));
            }
            return orders.ToArray();
        }

        public static OrderInfo ToOrderInfo(Order entityOrder, CFIEntities db)
        {
            OrderInfo order = new OrderInfo();
            order.ID = entityOrder.OrderID;
            order.Customer = ConversionUtils.ToCustomerInfo(entityOrder.Customer);
            order.Market = entityOrder.Store.Market.MarketName;
            if ( order.Market != null )
            {
                order.Market = order.Market.Trim();
            }
            order.PONumber = entityOrder.PurchaseOrderNumber.Trim();
            order.StoreNumber = entityOrder.Store.StoreNumber.Trim();

            // division assignment requires another lookup
            order.Division = getDivision(entityOrder.MaterialTypeID, db);

            // Diagrams
            if ( entityOrder.OrderDiagrams.Count > 0 )
            {
                try
                {
                    string installOrderNumber = entityOrder.CustomerOrderNumber.Trim();
                    string calcDateTimeStamp = entityOrder.OrderDiagrams.ElementAt<OrderDiagram>(0).DiagramDateTime.Value.ToString("MMddyyyyHHmm");
                    string diagramNumber = entityOrder.OrderDiagrams.ElementAt<OrderDiagram>(0).DiagramNumber;
                    string fileName = string.Format("{0}-{1}-{2}-{3}-{4}-1-0.pdf", order.StoreNumber, order.PONumber, installOrderNumber, diagramNumber, calcDateTimeStamp);
                    order.DiagramNumber = fileName;
                }
                catch (Exception ex)
                {
                    LogAPI.WebServiceLog.DebugFormat("failed to generate order diagram file name.  exception: {0}\r\n{1}", ex.Message, ex.StackTrace);
                    order.DiagramNumber = "";
                }
            }

            // if order is not scheduled then assign largest date value
            order.ScheduledDate = DateTime.MaxValue;
            if ((entityOrder.Scheduled == true) && (entityOrder.ScheduleStartDate != null))
            {
                order.ScheduledDate = entityOrder.ScheduleStartDate.Value;
            }

            // assign notes
            List<NoteInfo> notes = new List<NoteInfo>();
            foreach ( PONote poNote in entityOrder.PONotes )
            {
                if (poNote.Deleted == false)
                {
                    NoteInfo note = ConversionUtils.ToNoteInfo(poNote, db);
                    notes.Add(note);
                }
            }
            order.Notes = notes.ToArray();

            // assign photos
            List<PhotoInfo> photos = new List<PhotoInfo>();
            foreach (POPhoto poPhoto in entityOrder.POPhotos)
            {
                if (poPhoto.Deleted == false)
                {
                    PhotoInfo photo = ConversionUtils.ToPhotoInfo(poPhoto, db);
                    photos.Add(photo);
                }
            }
            order.Photos = photos.ToArray();
            
            return order;
        }

        public static NoteInfo ToNoteInfo(PONote poNote, CFIEntities db)
        {
            NoteInfo note = new NoteInfo();
            note.ID = poNote.ID;
            if (poNote.NoteText == null)
            {
                note.Text = "";
            }
            else
            {
                note.Text = poNote.NoteText.Trim();
            }
            note.TypeID = poNote.NoteTypeID;
            note.NoteTypeDescription = getNoteTypeDescription(poNote.NoteTypeID, db);
            note.DateTimeEntered = poNote.DateTimeEntered;
            if (poNote.DateTimeSent != null)
            {
                note.DateTimeSentToStore = poNote.DateTimeSent.Value;
            }
            else
            {
                note.DateTimeSentToStore = DateTime.MaxValue;
            }
            if (poNote.EnteredByUserID != null)
            {
                note.EnteredByUserID = poNote.EnteredByUserID.Value;
                note.EnteredByUser = getUserName(poNote.EnteredByUserID.Value, db);
            }
            else
            {
                note.EnteredByUserID = -1;
                note.EnteredByUser = null;
            }
            note.SentToStore = poNote.SentViaXML;
            return note;
        }

        public static NoteInfo[] ToNoteInfoArray(PONote[] poNotes, CFIEntities db)
        {
            List<NoteInfo> notes = new List<NoteInfo>();
            foreach (PONote poNote in poNotes)
            {
                if (poNote.Deleted == false)
                {
                    notes.Add(ToNoteInfo(poNote, db));
                }
            }
            return notes.ToArray();
        }

        public static PONote ToPONote(NoteInfo note, int orderID)
        {
            PONote poNote = new PONote();
            poNote.OrderID = orderID;
            poNote.NoteTypeID = note.TypeID;
            poNote.NoteText = note.Text;
            poNote.DateTimeEntered = note.DateTimeEntered;
            poNote.EnteredByUserID = note.EnteredByUserID;
            poNote.SentViaXML = note.SentToStore;
            if ( (note.DateTimeSentToStore == DateTime.MinValue) || ( note.DateTimeSentToStore == DateTime.MaxValue ) )
            {
                poNote.DateTimeSent = null;
            }
            else
            {
                poNote.DateTimeSent = note.DateTimeSentToStore;
            }
            return poNote;
        }

        public static NoteTypeInfo[] ToNoteTypeInfoArray(IEnumerable<NoteType> entityNoteTypes)
        {
            List<NoteTypeInfo> noteTypes = new List<NoteTypeInfo>();
            foreach (NoteType entityNoteType in entityNoteTypes)
            {
                noteTypes.Add(ToNoteTypeInfo(entityNoteType));
            }
            return noteTypes.ToArray();
        }

        public static NoteTypeInfo ToNoteTypeInfo(NoteType entityNoteType)
        {
            NoteTypeInfo noteType = new NoteTypeInfo();
            noteType.TypeID = entityNoteType.ID;
            noteType.Description = entityNoteType.NoteTypeDescription;
            return noteType;
        }

        public static UserInfo[] ToUserInfoArray(IEnumerable<Employee> entityEmployees)
        {
            List<UserInfo> users = new List<UserInfo>();
            foreach (Employee entityEmployee in entityEmployees)
            {
                users.Add(ToUserInfo(entityEmployee));
            }
            return users.ToArray();
        }

        public static UserInfo ToUserInfo(Employee entityEmployee)
        {
            UserInfo user = new UserInfo();
            user.ID = entityEmployee.ID;
            user.UserName = entityEmployee.UserName;
            return user;
        }
        
        public static PhotoInfo ToPhotoInfo(POPhoto poPhoto, CFIEntities db)
        {
            PhotoInfo photo = new PhotoInfo();
            photo.ID = poPhoto.ID;
            photo.FilePath = poPhoto.FilePath;
            photo.DateTimeEntered = poPhoto.DateTimeEntered;
            photo.Title = poPhoto.Title;

            if (poPhoto.EnteredByUserID != null)
            {
                photo.EnteredByUserID = poPhoto.EnteredByUserID.Value;
                photo.EnteredByUser = getUserName(poPhoto.EnteredByUserID.Value, db);
            }
            else
            {
                photo.EnteredByUserID = -1;
                photo.EnteredByUser = null;
            }

            return photo;
        }

        public static PhotoInfo[] ToPhotoInfoArray(POPhoto[] poPhotos, CFIEntities db)
        {
            List<PhotoInfo> photos = new List<PhotoInfo>();
            foreach (POPhoto poPhoto in poPhotos)
            {
                if (poPhoto.Deleted == false)
                {
                    photos.Add(ToPhotoInfo(poPhoto, db));
                }
            }
            return photos.ToArray();
        }

        public static POPhoto ToPOPhoto(PhotoInfo photo, int orderID)
        {
            POPhoto poPhoto = new POPhoto();
            poPhoto.OrderID = orderID;
            poPhoto.Title = photo.Title;
            poPhoto.FilePath = photo.FilePath;
            poPhoto.DateTimeEntered = photo.DateTimeEntered;
            poPhoto.EnteredByUserID = photo.EnteredByUserID;
            return poPhoto;
        }

        private static Dictionary<int, string> divisionTable = null;
        private static object divisionTableDataLock = new object();
        private static string getDivision(int materialID, CFIEntities db)
        {
            lock (divisionTableDataLock)
            {
                // the table can be updated out of band to this application so check for the presence of the ID.  If the ID key 
                // is missing then rebuild the table.
                if ( (divisionTable == null) || ( divisionTable.ContainsKey(materialID) == false ) )
                {
                    buildDivisionTable(db);
                }

                try
                {
                    return divisionTable[materialID];
                }
                catch
                {
                    LogAPI.WebServiceLog.ErrorFormat("Unexpected MaterialID {0} encountered during Division lookup", materialID);
                    return "UNKNOWN DIVISION";
                }
            }
        }

        private static void buildDivisionTable(CFIEntities db)
        {
            divisionTable = new Dictionary<int, string>();

            var query =
                from m in db.MaterialTypes
                select m;

            foreach ( MaterialType materialType in query.ToArray<MaterialType>() )
            {
                string division = materialType.Division.Division1;
                if ( string.IsNullOrEmpty(division) == false )
                {
                    division = division.Trim();
                }
                divisionTable.Add( materialType.MaterialTypeID, division );
            }
        }

        private static Dictionary<int, string> noteTypeTable = null;
        private static object noteTypeTableDataLock = new object();
        private static string getNoteTypeDescription(int noteTypeID, CFIEntities db)
        {
            lock (noteTypeTableDataLock)
            {
                // the table can be updated out of band to this application so check for the presence of the ID.  If the ID key 
                // is missing then rebuild the table.
                if ((noteTypeTable == null) || (noteTypeTable.ContainsKey(noteTypeID) == false))
                {
                    buildNoteTypeTable(db);
                }

                try
                {
                    return noteTypeTable[noteTypeID];
                }
                catch
                {
                    LogAPI.WebServiceLog.ErrorFormat("Unexpected noteTypeID {0} encountered during noteType lookup", noteTypeID);
                    return "UNKNOWN Note Type";
                }
            }
        }

        private static void buildNoteTypeTable(CFIEntities db)
        {
            noteTypeTable = new Dictionary<int, string>();

            var query =
                from n in db.NoteTypes
                select n;

            foreach (NoteType noteType in query.ToArray<NoteType>())
            {
                noteTypeTable.Add( noteType.ID, noteType.NoteTypeDescription );
            }
        }

        private static Dictionary<int, string> userIDTable = null;
        private static object userIDTableDataLock = new object();
        private static string getUserName(int userID, CFIEntities db)
        {
            lock (userIDTableDataLock)
            {
                // the table can be updated out of band to this application so check for the presence of the ID.  If the ID key 
                // is missing then rebuild the table.
                if ((userIDTable == null) || (userIDTable.ContainsKey(userID) == false))
                {
                    buildUserIDTable(db);
                }

                try
                {
                    return userIDTable[userID];
                }
                catch
                {
                    LogAPI.WebServiceLog.ErrorFormat("Unexpected UserID {0} encountered during User Name lookup", userID);
                    return "UNKNOWN user ID";
                }
            }
        }

        private static void buildUserIDTable(CFIEntities db)
        {
            userIDTable = new Dictionary<int, string>();

            var query =
                from e in db.Employees
                select e;

            foreach (Employee employee in query.ToArray<Employee>())
            {
                string fullName;
                if ( string.IsNullOrEmpty(employee.FirstName) == true )
                {
                    fullName = employee.LastName;
                }
                else
                {
                    fullName = string.Format("{0}, {1}", employee.LastName, employee.FirstName);
                }
                userIDTable.Add(employee.ID, fullName);
            }
        }

    }
}