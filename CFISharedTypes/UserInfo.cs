using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace CFI
{
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        private static string usersTag = "Users";
        private static string userTag = "User";
        private static string idTag = "ID";
        private static string userNameTag = "UserName";

        public static string BuildUsersXml(UserInfo[] users)
        {
            XmlTextWriter writer = XmlAPI.CreateWriter();
            writer.WriteStartElement(usersTag);
            foreach (UserInfo user in users)
            {
                writeUserXml(writer, user);
            }
            writer.WriteEndElement();
            return XmlAPI.FlushWriter(writer);
        }

        public static UserInfo[] ParseUsersXml(string xml)
        {
            try
            {
                List<UserInfo> users = new List<UserInfo>();

                // get the root element
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlElement usersElement = document.SelectSingleNode(usersTag) as XmlElement;

                XmlNodeList nodes = usersElement.GetElementsByTagName(userTag);
                if ((nodes != null) && (nodes.Count > 0))
                {
                    foreach (XmlNode userNode in nodes)
                    {
                        XmlElement userElement = userNode as XmlElement;
                        UserInfo user = parseUser(userElement);
                        users.Add(user);
                    }
                }
                return users.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private static UserInfo parseUser(XmlElement userElement)
        {
            UserInfo user = new UserInfo();
            user.ID = int.Parse(userElement.GetElementsByTagName(idTag)[0].InnerText);
            user.UserName = userElement.GetElementsByTagName(userNameTag)[0].InnerText;
            return user;
        }

        private static void writeUserXml(XmlTextWriter writer, UserInfo user)
        {
            writer.WriteStartElement(userTag);

            writer.WriteElementString(idTag, user.ID.ToString());

            writer.WriteStartElement(userNameTag);
            writer.WriteCData(user.UserName);
            writer.WriteEndElement();

            writer.WriteEndElement();

        }

    }
}
