using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace CFI
{
    [DataContract]
    public class NoteTypeInfo
    {
        [DataMember]
        public int TypeID { get; set; }

        [DataMember]
        public string Description { get; set; }

        private static string noteTypesTag = "NoteTypes";
        private static string noteTypeTag = "NoteType";
        private static string typeIDTag = "TypeID";
        private static string DescriptionTag = "Description";

        public static string BuildNoteTypesXml(NoteTypeInfo[] noteTypes)
        {
            XmlTextWriter writer = XmlAPI.CreateWriter();
            writer.WriteStartElement(noteTypesTag);
            foreach (NoteTypeInfo noteType in noteTypes)
            {
                writeNoteTypeXml(writer, noteType);
            }
            writer.WriteEndElement();
            return XmlAPI.FlushWriter(writer);
        }

        public static NoteTypeInfo[] ParseNoteTypesXml(string xml)
        {
            try
            {
                List<NoteTypeInfo> noteTypes = new List<NoteTypeInfo>();

                // get the root element
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlElement noteTypesElement = document.SelectSingleNode(noteTypesTag) as XmlElement;

                XmlNodeList nodes = noteTypesElement.GetElementsByTagName(noteTypeTag);
                if ((nodes != null) && (nodes.Count > 0))
                {
                    foreach (XmlNode noteTypeNode in nodes)
                    {
                        XmlElement noteTypeElement = noteTypeNode as XmlElement;
                        NoteTypeInfo noteType = parseNoteType(noteTypeElement);
                        noteTypes.Add(noteType);
                    }
                }
                return noteTypes.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private static NoteTypeInfo parseNoteType(XmlElement noteTypeElement)
        {
            NoteTypeInfo noteType = new NoteTypeInfo();
            noteType.TypeID = int.Parse(noteTypeElement.GetElementsByTagName(typeIDTag)[0].InnerText);
            noteType.Description = noteTypeElement.GetElementsByTagName(DescriptionTag)[0].InnerText;
            return noteType;
        }

        private static void writeNoteTypeXml(XmlTextWriter writer, NoteTypeInfo noteType)
        {
            writer.WriteStartElement(noteTypeTag);

            writer.WriteElementString(typeIDTag, noteType.TypeID.ToString());

            writer.WriteStartElement(DescriptionTag);
            writer.WriteCData(noteType.Description);
            writer.WriteEndElement();

            writer.WriteEndElement();

        }

    }
}
