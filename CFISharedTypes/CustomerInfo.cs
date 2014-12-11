using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace CFI
{
    [DataContract]
    public class CustomerInfo
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        private static string customersTag = "Customers";
        private static string customerTag = "Customer";
        private static string idTag = "ID";
        private static string LastNameTag = "LastName";
        private static string FirstNameTag = "FirstName";

        public override string ToString()
        {
            string retVal;
            if ((string.IsNullOrEmpty(LastName) == false) &&
                (string.IsNullOrEmpty(FirstName) == false))
            {
                retVal = string.Format("{0}, {1}", LastName, FirstName);
            }
            else if (string.IsNullOrEmpty(FirstName))
            {
                retVal = LastName;
            }
            else
            {
                retVal = FirstName;
            }
            return retVal;
        }

        public static string BuildCustomersXml(CustomerInfo[] customers)
        {
            XmlTextWriter writer = XmlAPI.CreateWriter();
            writer.WriteStartElement(customersTag);
            foreach ( CustomerInfo customer in customers )
            {
                WriteCustomerXml(writer, customer);
            }
            writer.WriteEndElement();
            return XmlAPI.FlushWriter(writer);
        }

        public static CustomerInfo[] ParseCustomersXml(string xml)
        {
            try
            {
                List<CustomerInfo> customers = new List<CustomerInfo>();

                // get the root element
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlElement customersElement = document.SelectSingleNode(customersTag) as XmlElement;

                XmlNodeList nodes = customersElement.GetElementsByTagName(customerTag);
                if ((nodes != null) && (nodes.Count > 0))
                {
                    foreach (XmlNode customerNode in nodes)
                    {
                        XmlElement customerElement = customerNode as XmlElement;
                        CustomerInfo customer = ParseCustomer( customerElement );
                        customers.Add(customer);
                    }
                }
                return customers.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public static CustomerInfo ParseCustomer(XmlElement customerElement)
        {
            CustomerInfo customer = new CustomerInfo();
            customer.ID = int.Parse( customerElement.GetElementsByTagName(idTag)[0].InnerText );
            customer.LastName = customerElement.GetElementsByTagName(LastNameTag)[0].InnerText;
            customer.FirstName = customerElement.GetElementsByTagName(FirstNameTag)[0].InnerText;
            return customer;
        }

        public static void WriteCustomerXml(XmlTextWriter writer, CustomerInfo customer)
        {
            writer.WriteStartElement(customerTag);

            writer.WriteElementString(idTag, customer.ID.ToString());

            writer.WriteStartElement(LastNameTag);
            writer.WriteCData(customer.LastName);
            writer.WriteEndElement();

            writer.WriteStartElement(FirstNameTag);
            writer.WriteCData(customer.FirstName);
            writer.WriteEndElement();

            writer.WriteEndElement();

        }
    }
}
