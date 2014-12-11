using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace CFI
{
    [DataContract]
    public class PhotoInfo : IEquatable<PhotoInfo>
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public DateTime DateTimeEntered { get; set; }

        [DataMember]
        public int EnteredByUserID { get; set; }

        // redundant representation to User ID
        [DataMember]
        public string EnteredByUser { get; set; }

        public bool Equals(PhotoInfo other)
        {
            return ((this.Title == other.Title) &&
                    ( this.DateTimeEntered.ToString() == other.DateTimeEntered.ToString() ) &&
                    ( this.EnteredByUserID == other.EnteredByUserID ) );
        }

        public string DebugText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Title:                   {0}\r\n", this.Title);
                sb.AppendFormat("FilePath                 {0}\r\n", this.FilePath);
                sb.AppendFormat("Entered By:              {0} (User ID {1})\r\n", this.EnteredByUser, this.EnteredByUserID);
                sb.AppendFormat("Date/Time Entered:       {0}\r\n", this.DateTimeEntered);
                sb.AppendFormat("ID:                      {0}\r\n", this.ID);
                return sb.ToString();
            }
        }


        private static string photosTag = "Photos";
        private static string photoTag = "Photo";
        private static string idTag = "ID";
        private static string titleTag = "Title";
        private static string filePathTag = "FilePath";
        private static string dateTimeEnteredTag = "DateTimeEntered";
        private static string enteredByUserIDTag = "EnteredByUserID";
        private static string enteredByUserTag = "EnteredByUser";

        public static string BuildPhotosXml(PhotoInfo[] photos)
        {
            XmlTextWriter writer = XmlAPI.CreateWriter();
            WritePhotos(writer, photos, photosTag);
            return XmlAPI.FlushWriter(writer);
        }

        public static void WritePhotos(XmlTextWriter writer, PhotoInfo[] photos, string containerTagName)
        {
            writer.WriteStartElement(containerTagName);
            foreach (PhotoInfo photo in photos)
            {
                WritePhotoXml(writer, photo);
            }
            writer.WriteEndElement();
        }

        public static PhotoInfo[] ParsePhotos(XmlElement photosElement)
        {
            try
            {
                List<PhotoInfo> photos = new List<PhotoInfo>();
                XmlNodeList nodes = photosElement.GetElementsByTagName( photoTag );
                foreach ( XmlNode node in nodes )
                {
                    XmlElement photoElement = node as XmlElement;
                    photos.Add(PhotoInfo.ParsePhoto(photoElement));
                }
                return photos.ToArray();
            }
            catch
            {
                return null;
            }
        }


        public static PhotoInfo[] ParsePhotosXml(string xml)
        {
            try
            {
                List<PhotoInfo> photos = new List<PhotoInfo>();

                // get the root element
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlElement photosElement = document.SelectSingleNode(photosTag) as XmlElement;

                XmlNodeList nodes = photosElement.GetElementsByTagName(photoTag);
                if ((nodes != null) && (nodes.Count > 0))
                {
                    foreach (XmlNode photoNode in nodes)
                    {
                        XmlElement photoElement = photoNode as XmlElement;
                        PhotoInfo photo = ParsePhoto(photoElement);
                        photos.Add(photo);
                    }
                }
                return photos.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static PhotoInfo ParsePhoto(XmlElement photoElement)
        {
            PhotoInfo photo = new PhotoInfo();
            photo.ID = int.Parse(photoElement.GetElementsByTagName(idTag)[0].InnerText);
            photo.Title = photoElement.GetElementsByTagName(titleTag)[0].InnerText;
            photo.FilePath = photoElement.GetElementsByTagName(filePathTag)[0].InnerText;
            photo.DateTimeEntered = DateTime.Parse(photoElement.GetElementsByTagName(dateTimeEnteredTag)[0].InnerText);
            photo.EnteredByUserID = int.Parse(photoElement.GetElementsByTagName(enteredByUserIDTag)[0].InnerText);
            photo.EnteredByUser = photoElement.GetElementsByTagName(enteredByUserTag)[0].InnerText;
            return photo;
        }

        public static void WritePhotoXml(XmlTextWriter writer, PhotoInfo photo)
        {
            writer.WriteStartElement(photoTag);
            writer.WriteElementString(idTag, photo.ID.ToString());

            writer.WriteStartElement(titleTag);
            writer.WriteCData( photo.Title );
            writer.WriteEndElement();

            writer.WriteElementString(filePathTag, photo.FilePath);
            writer.WriteElementString(dateTimeEnteredTag, photo.DateTimeEntered.ToString());
            writer.WriteElementString(enteredByUserIDTag, photo.EnteredByUserID.ToString());
            writer.WriteElementString(enteredByUserTag, photo.EnteredByUser);
            writer.WriteEndElement();
        }
    }
}
