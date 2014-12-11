using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace CFI
{
    [DataContract]
    public class NoteInfo : IEquatable<NoteInfo>
    {
        [DataMember]
        public int ID { get; set; }
        
        [DataMember]
        public string Text { get; set; }

        // deliberately redundant representations
        [DataMember]
        public int TypeID { get; set; }

        [DataMember]
        public string NoteTypeDescription { get; set; }

        [DataMember]
        public DateTime DateTimeEntered { get; set; }

        [DataMember]
        public DateTime DateTimeSentToStore { get; set; }

        [DataMember]
        public bool SentToStore { get; set; }

        [DataMember]
        public int EnteredByUserID { get; set; }

        // redundant representation to User ID
        [DataMember]
        public string EnteredByUser { get; set; }

        public bool Equals(NoteInfo other)
        {
            return ( ( this.Text == other.Text ) &&
                     ( this.TypeID == other.TypeID ) &&
                     ( this.DateTimeEntered.ToString() == other.DateTimeEntered.ToString() ) &&
                     ( this.EnteredByUserID == other.EnteredByUserID ) );
        }

        public string DebugText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[ {0} ]\r\n", this.NoteTypeDescription);
                sb.AppendFormat("Entered By:              {0} (User ID {1})\r\n", this.EnteredByUser, this.EnteredByUserID);
                sb.AppendFormat("Sent To Store:           {0}\r\n", this.SentToStore ? "Yes" : "No" );
                sb.AppendFormat("Text:                    {0}\r\n", this.Text);
                sb.AppendFormat("Date/Time Entered:       {0}\r\n", this.DateTimeEntered);
                if ( this.SentToStore )
                {
                    sb.AppendFormat("Date/Time Sent to store: {0}\r\n", this.DateTimeSentToStore);
                }
                sb.AppendFormat("ID:                      {0}\r\n", this.ID);
                return sb.ToString();
            }
        }


        private static string notesTag = "Notes";
        private static string noteTag = "Note";
        private static string idTag = "ID";
        private static string textTag = "Text";
        private static string typeIDTag = "TypeID";
        private static string typeDescriptionTag = "TypeDescription";
        private static string dateTimeEnteredTag = "DateTimeEntered";
        private static string dateTimeSentToStoreTag = "DateTimeSentToStore";
        private static string enteredByUserIDTag = "EnteredByUserID";
        private static string enteredByUserTag = "EnteredByUser";
        private static string sentToStoreTag = "SentToStore";

        public static string BuildNotesXml(NoteInfo[] notes)
        {
            XmlTextWriter writer = XmlAPI.CreateWriter();
            WriteNotes(writer, notes, notesTag);
            return XmlAPI.FlushWriter(writer);
        }

        public static void WriteNotes(XmlTextWriter writer, NoteInfo[] notes, string containerTagName)
        {
            writer.WriteStartElement(containerTagName);
            foreach (NoteInfo note in notes)
            {
                WriteNoteXml(writer, note);
            }
            writer.WriteEndElement();
        }

        public static NoteInfo[] ParseNotes(XmlElement notesElement)
        {
            try
            {
                List<NoteInfo> notes = new List<NoteInfo>();
                XmlNodeList nodes = notesElement.GetElementsByTagName(noteTag);
                foreach ( XmlNode node in nodes )
                {
                    XmlElement noteElement = node as XmlElement;
                    notes.Add( NoteInfo.ParseNote( noteElement ) );
                }
                return notes.ToArray();
            }
            catch
            {
                return null;
            }
        }


        public static NoteInfo[] ParseNotesXml(string xml)
        {
            try
            {
                List<NoteInfo> notes = new List<NoteInfo>();

                // get the root element
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlElement notesElement = document.SelectSingleNode(notesTag) as XmlElement;

                XmlNodeList nodes = notesElement.GetElementsByTagName(noteTag);
                if ((nodes != null) && (nodes.Count > 0))
                {
                    foreach (XmlNode noteNode in nodes)
                    {
                        XmlElement noteElement = noteNode as XmlElement;
                        NoteInfo note = ParseNote(noteElement);
                        notes.Add(note);
                    }
                }
                return notes.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static NoteInfo ParseNote(XmlElement noteElement)
        {
            NoteInfo note = new NoteInfo();
            note.ID = int.Parse(noteElement.GetElementsByTagName(idTag)[0].InnerText);
            note.TypeID = int.Parse(noteElement.GetElementsByTagName(typeIDTag)[0].InnerText);
            note.Text = noteElement.GetElementsByTagName(textTag)[0].InnerText;
            note.NoteTypeDescription = noteElement.GetElementsByTagName(typeDescriptionTag)[0].InnerText;

            note.DateTimeEntered = DateTime.Parse( noteElement.GetElementsByTagName(dateTimeEnteredTag)[0].InnerText );
            note.DateTimeSentToStore = DateTime.Parse( noteElement.GetElementsByTagName(dateTimeSentToStoreTag)[0].InnerText );
            note.SentToStore = bool.Parse( noteElement.GetElementsByTagName(sentToStoreTag)[0].InnerText );

            note.EnteredByUserID = int.Parse(noteElement.GetElementsByTagName(enteredByUserIDTag)[0].InnerText);
            note.EnteredByUser = noteElement.GetElementsByTagName(enteredByUserTag)[0].InnerText;

            return note;
        }

        public static void WriteNoteXml(XmlTextWriter writer, NoteInfo note)
        {
            writer.WriteStartElement(noteTag);

            writer.WriteElementString(idTag, note.ID.ToString());
            writer.WriteElementString(typeIDTag, note.TypeID.ToString());

            writer.WriteStartElement(textTag);
            writer.WriteCData(note.Text);
            writer.WriteEndElement();

            writer.WriteStartElement(typeDescriptionTag);
            writer.WriteCData(note.NoteTypeDescription);
            writer.WriteEndElement();

            writer.WriteElementString(dateTimeEnteredTag, note.DateTimeEntered.ToString());
            writer.WriteElementString(dateTimeSentToStoreTag, note.DateTimeSentToStore.ToString());
            writer.WriteElementString(sentToStoreTag, note.SentToStore.ToString());

            writer.WriteElementString(enteredByUserIDTag, note.EnteredByUserID.ToString());
            writer.WriteElementString(enteredByUserTag, note.EnteredByUser);

            writer.WriteEndElement();

        }

    }
}
