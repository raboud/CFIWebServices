using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Xml;
using CFI;

namespace CFI.Client
{
	public class CacheMetaData
	{
		private ConcurrentDictionary<int, string> userIdNameTable = new ConcurrentDictionary<int, string>();
		private ConcurrentDictionary<int, string> noteTypeIdDescriptionTable = new ConcurrentDictionary<int, string>();

        // XML element names
        private const string metaDataTag = "CacheMetaData";
        private const string usersTag = "Users";
        private const string userTag = "User";
        private const string userIdTag = "UserID";
        private const string userNameTag = "UserName";
        private const string noteTypesTag = "NoteTypes";
        private const string noteTypeTag = "NoteType";
        private const string noteTypeIdTag = "NoteTypeID";
        private const string noteTypeDescriptionTag = "Description";

        public int NumUsers
        {
            get
            {
                lock (this)
                {
                    return this.userIdNameTable.Count;
                }
            }
        }

        public int NumNoteTypes
        {
            get
            {
                lock (this)
                {
                    return this.noteTypeIdDescriptionTable.Count;
                }
            }
        }

        public void AssignUsers(UserInfo[] users)
        {
            lock (this)
            {
                // clear existing table
                userIdNameTable = new ConcurrentDictionary<int, string>();
                foreach ( UserInfo user in users )
                {
                    AddUser(user.ID, user.UserName);
                }

            }
        }

        public void AssignNoteTypes(NoteTypeInfo[] noteTypes)
        {
            lock (this)
            {
                // clear existing table
                noteTypeIdDescriptionTable = new ConcurrentDictionary<int, string>();
                foreach (NoteTypeInfo noteType in noteTypes)
                {
                    AddNoteType(noteType.TypeID, noteType.Description);
                }
            }
        }

        public void AddUser(int id, string userName)
		{
            lock (this)
            {
                userIdNameTable[id] = userName;
            }
		}
		
		public void AddNoteType( int id, string description )
		{
            lock (this)
            {
                noteTypeIdDescriptionTable[id] = description;
            }
        }

        public int[] GetUserIDs()
        {
            lock (this)
            {
                List<int> ids = new List<int>(this.userIdNameTable.Keys);
                ids.Sort();
                return ids.ToArray();
            }
        }

		public string[] GetUserNames()
		{
            lock (this)
            {
                List<string> names = new List<string>(userIdNameTable.Values);
                names.Sort();
                return names.ToArray();
            }
		}
		
		public int GetUserID( string userName )
		{
            lock (this)
            {
                foreach (KeyValuePair<int, string> pair in userIdNameTable)
                {
                    if (string.Compare(userName, pair.Value, true) == 0)
                    {
                        return pair.Key;
                    }
                }
                return -1;
            }
		}
		
		public string GetUserName( int id )
		{
            lock (this)
            {
                if (userIdNameTable.ContainsKey(id))
                {
                    return userIdNameTable[id];
                }
                else
                {
                    return null;
                }
            }
		}

        public int[] GetNoteTypeIDs()
        {
            lock (this)
            {
                List<int> ids = new List<int>(this.noteTypeIdDescriptionTable.Keys);
                ids.Sort();
                return ids.ToArray();
            }
        }

		public int GetNoteTypeID( string description )
		{
            lock (this)
            {
                foreach (KeyValuePair<int, string> pair in noteTypeIdDescriptionTable)
                {
                    if (string.Compare(description, pair.Value, true) == 0)
                    {
                        return pair.Key;
                    }
                }
                return -1;
            }
		}
		
		public string GetNoteTypeDescription( int id )
		{
            lock (this)
            {
                if (noteTypeIdDescriptionTable.ContainsKey(id))
                {
                    return noteTypeIdDescriptionTable[id];
                }
                else
                {
                    return null;
                }
            }
		}

        public string[] GetNoteTypeDescriptions()
        {
            lock (this)
            {
                List<string> descriptions = new List<string>(this.noteTypeIdDescriptionTable.Values);
                descriptions.Sort();
                return descriptions.ToArray();
            }
        }

        private class IntStringPairSorter : IComparer<KeyValuePair<int, string>>
        {
            public int Compare(KeyValuePair<int, string> x, KeyValuePair<int, string> y)
            {
                int result = x.Key.CompareTo(y.Key);
                if (result != 0)
                {
                    return result;
                }
                else
                {
                    return x.Value.CompareTo(y.Value);    
                }
            }
        }

		public string ToXml()
		{
            lock (this)
            {
                XmlTextWriter writer = XmlAPI.CreateWriter();

                // enclosing element
                writer.WriteStartElement(metaDataTag);

                // users element
                writer.WriteStartElement(usersTag);

                // sort the pairs by id
                List<KeyValuePair<int, string>> pairs = new List<KeyValuePair<int, string>>(userIdNameTable);
                pairs.Sort(new IntStringPairSorter());
                foreach (KeyValuePair<int, string> pair in pairs)
                {
                    writer.WriteStartElement(userTag);
                    writer.WriteElementString(userIdTag, pair.Key.ToString());
                    writer.WriteStartElement(userNameTag);
                    writer.WriteCData(pair.Value);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                // note types element
                writer.WriteStartElement(noteTypesTag);

                pairs = new List<KeyValuePair<int, string>>(noteTypeIdDescriptionTable);
                pairs.Sort(new IntStringPairSorter());
                foreach (KeyValuePair<int, string> pair in pairs)
                {
                    writer.WriteStartElement(noteTypeTag);
                    writer.WriteElementString(noteTypeIdTag, pair.Key.ToString());
                    writer.WriteStartElement(noteTypeDescriptionTag);
                    writer.WriteCData(pair.Value);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
                return XmlAPI.FlushWriter(writer);
            }
		}
		
        public static CacheMetaData ParseXml(string xml)
        {
            try
            {
                CacheMetaData metaData = new CacheMetaData();

                // get the root element
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlElement rootElement = document.SelectSingleNode(metaDataTag) as XmlElement;

                // get the users
                XmlElement usersElement = rootElement.SelectSingleNode(usersTag) as XmlElement;
                XmlNodeList nodes = usersElement.GetElementsByTagName(userTag);
                if ((nodes != null) && (nodes.Count > 0))
                {
                    foreach (XmlNode userNode in nodes)
                    {
                        XmlElement userElement = userNode as XmlElement;
                        XmlElement userIDElement = userElement.SelectSingleNode(userIdTag) as XmlElement;
                        int userID = int.Parse(userIDElement.InnerText);
                        XmlElement userNameElement = userElement.SelectSingleNode(userNameTag) as XmlElement;
                        string userName = userNameElement.InnerText;
                        metaData.AddUser(userID, userName);
                    }
                }

                // get the note types
                XmlElement noteTypesElement = rootElement.SelectSingleNode(noteTypesTag) as XmlElement;
                nodes = noteTypesElement.GetElementsByTagName(noteTypeTag);
                if ((nodes != null) && (nodes.Count > 0))
                {
                    foreach (XmlNode noteTypeNode in nodes)
                    {
                        XmlElement noteTypeElement = noteTypeNode as XmlElement;
                        XmlElement noteTypeIDElement = noteTypeElement.SelectSingleNode(noteTypeIdTag) as XmlElement;
                        int noteTypeID = int.Parse(noteTypeIDElement.InnerText);
                        XmlElement noteTypeDescriptionElement = noteTypeElement.SelectSingleNode(noteTypeDescriptionTag) as XmlElement;
                        string description = noteTypeDescriptionElement.InnerText;
                        metaData.AddNoteType(noteTypeID, description);
                    }
                }

                return metaData;
            }
            catch
            {
                return null;
            }
        }

        public static bool AreEquivalent(CacheMetaData md1, CacheMetaData md2)
        {
            if ( ( md1.NumUsers != md2.NumUsers ) || ( md1.NumNoteTypes != md2.NumNoteTypes ) )
            {
                return false;
            }
            foreach ( int id in md1.GetUserIDs() )
            {
                try
                {
                    if ( md1.GetUserName(id) != md2.GetUserName(id) )
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            foreach (int id in md1.GetNoteTypeIDs())
            {
                try
                {
                    if (md1.GetNoteTypeDescription(id) != md2.GetNoteTypeDescription(id))
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}

