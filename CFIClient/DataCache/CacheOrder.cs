using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using CFI;

namespace CFI.Client
{
    public class CacheOrder : IEquatable<CacheOrder>
    {
        private const string orderTag = "Order";
        private const string statusTag = "Status";
        private const string existingNotesTag = "ExistingNotes";
        private const string newNotesTag = "NewNotes";
        private const string existingPhotosTag = "ExistingPhotos";
        private const string newPhotosTag = "NewPhotos";

        private int nextNewNoteIndex = 0;
        private int nextNewPhotoIndex = 0;

        private OrderInfo order;
        public OrderInfo Order
        {
            get { return order; }
        }

        private List<NoteInfo> newNotes = new List<NoteInfo>();
        public NoteInfo[] NewNotes
        {
            get
            {
                return newNotes.ToArray();
            }
        }

        private List<PhotoInfo> newPhotos = new List<PhotoInfo>();
        public PhotoInfo[] NewPhotos
        {
            get
            {
                return newPhotos.ToArray();
            }
        }

        private CacheStatus status;
        public CacheStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        private CacheOrder() { }

        public CacheOrder (OrderInfo order, NoteInfo[] notesToAdd, PhotoInfo[] photosToAdd, CacheStatus status)
        {
            this.order = order;

            this.newNotes = new List<NoteInfo>();
            if (notesToAdd != null)
            {
                // reassign the IDs to linearlize them in ascending order
                foreach ( NoteInfo note in notesToAdd)
                {
                    note.ID = nextNewNoteIndex;
                    this.newNotes.Add(note);
                    nextNewNoteIndex++;
                }
            }

            this.newPhotos = new List<PhotoInfo>();
            if (photosToAdd != null)
            {
                // reassign the IDs to linearlize them in ascending order
                foreach (PhotoInfo photo in photosToAdd)
                {
                    photo.ID = nextNewPhotoIndex;
                    this.newPhotos.Add(photo);
                    nextNewPhotoIndex++;
                }
            }
            
            this.status = status;
        }

        public void AddNewNote(NoteInfo note)
        {
            // fix up the note ID so that it is the last one in the list
            note.ID = nextNewNoteIndex;
            newNotes.Add(note);
            nextNewNoteIndex++;
            this.status = CacheStatus.Dirty;
        }

        public void DeleteNewNote(int id)
        {
            List<NoteInfo> notesNotDeleted = new List<NoteInfo>();
            foreach ( NoteInfo newNote in this.newNotes )
            {
                if ( newNote.ID != id )
                {
                    notesNotDeleted.Add(newNote);
                }
            }
            this.newNotes = notesNotDeleted;
            this.status = CacheStatus.Dirty;
        }

        public void UpdateNewNote( int id, string newText )
        {
            foreach ( NoteInfo note in this.newNotes )
            {
                if ( note.ID == id )
                {
                    note.Text = newText;
                    this.status = CacheStatus.Dirty;
                    return;
                }
            }
        }

        public void AddNewPhoto( string title, string cachedPhotoFullPath, int userID, string userName )
        {
            // write info object that refers to image
            PhotoInfo photo = new PhotoInfo();
            photo.FilePath = cachedPhotoFullPath;
            photo.Title = title;
            photo.DateTimeEntered = DateTime.Now;
            photo.EnteredByUserID = userID;
			photo.EnteredByUser = userName;

			// fix up the photo ID so that it is the last one in the list
            photo.ID = nextNewPhotoIndex;
			
			newPhotos.Add(photo);
            nextNewPhotoIndex++;

            this.status = CacheStatus.Dirty;
        }
		
        public void DeleteNewPhoto(int id)
        {
            // we deliberately orphan the cached file because it may not be in the cache folder anyway.  Also,
            // the entire folder will be cleared when the order is deleted.

            List<PhotoInfo> photosNotDeleted = new List<PhotoInfo>();
            foreach (PhotoInfo newPhoto in this.newPhotos)
            {
                if (newPhoto.ID != id)
                {
                    photosNotDeleted.Add(newPhoto);
                }
            }
            this.newPhotos = photosNotDeleted;
            
            this.status = CacheStatus.Dirty;
        }
         
        public bool Equals(CacheOrder other)
        {
            return ( ( this.Order.Equals(other.Order) ) &&
                     ( this.Status == other.Status) &&
                     ( sameNotes( this.NewNotes, other.NewNotes ) ) && 
                     ( samePhotos( this.NewPhotos, other.NewPhotos ) ) );
        }

        private bool sameNotes(NoteInfo[] notes1, NoteInfo[] notes2)
        {
            if ( notes1.Length != notes2.Length )
            {
                return false;
            }

            for (int i = 0; i < notes1.Length; i++ )
            {
                if ( notes1[i].Equals(notes2[i]) == false )
                {
                    return false;
                }
            }

            return true;
        }

        private bool samePhotos(PhotoInfo[] photos1, PhotoInfo[] photos2)
        {
            if (photos1.Length != photos2.Length)
            {
                return false;
            }

            for (int i = 0; i < photos1.Length; i++)
            {
                if (photos1[i].Equals(photos2[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }


        public string ToXml()
        {
            XmlTextWriter writer = XmlAPI.CreateWriter();
            writer.WriteStartElement(orderTag);

            // write the non-note PO details directly from the object
            this.Order.WriteOrderDetailsXml(writer);

            NoteInfo.WriteNotes(writer, this.Order.Notes, existingNotesTag);
            NoteInfo.WriteNotes(writer, this.NewNotes, newNotesTag);

            PhotoInfo.WritePhotos(writer, this.Order.Photos, existingPhotosTag);
            PhotoInfo.WritePhotos(writer, this.NewPhotos, newPhotosTag);

            writer.WriteElementString(statusTag, this.Status.ToString());

            writer.WriteEndElement();
            return XmlAPI.FlushWriter(writer);
        }

        public static CacheOrder ParseXml(string xml)
        {
            try
            {
                // parse the XML into a document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlElement orderElement = doc.GetElementsByTagName(orderTag)[0] as XmlElement;

                // create the pieces from XML elements
                OrderInfo order = new OrderInfo();
                order.ParseOrderDetailsXml(orderElement);

                XmlElement existingNotesElement = orderElement.GetElementsByTagName(existingNotesTag)[0] as XmlElement;
                order.Notes = NoteInfo.ParseNotes(existingNotesElement);

                XmlElement newNotesElement = orderElement.GetElementsByTagName(newNotesTag)[0] as XmlElement;
                NoteInfo[] newNotes = NoteInfo.ParseNotes(newNotesElement);

                XmlElement existingPhotosElement = orderElement.GetElementsByTagName(existingPhotosTag)[0] as XmlElement;
                order.Photos = PhotoInfo.ParsePhotos(existingPhotosElement);

                XmlElement newPhotosElement = orderElement.GetElementsByTagName(newPhotosTag)[0] as XmlElement;
                PhotoInfo[] newPhotos = PhotoInfo.ParsePhotos(newPhotosElement);

                XmlElement statusElement = orderElement.GetElementsByTagName(statusTag)[0] as XmlElement;
                CacheStatus status = (CacheStatus)Enum.Parse(typeof(CacheStatus), statusElement.InnerText);

                // create the cache order object from the pieces
                return new CacheOrder(order, newNotes, newPhotos, status);
            }
            catch
            {
                return null;
            }
        }

    }
}
