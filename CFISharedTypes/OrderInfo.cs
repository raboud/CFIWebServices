using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace CFI
{
    [DataContract]
    public class OrderInfo : IEquatable<OrderInfo>
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public CustomerInfo Customer { get; set; }

        [DataMember]
        public string PONumber { get; set; } // 8 digits

        [DataMember]
        public string StoreNumber { get; set; } // 4 digits
        
        [DataMember]
        public DateTime ScheduledDate = DateTime.MaxValue;

        [DataMember]
        public string DiagramNumber { get; set; }
        
        [DataMember]
        public string NotesXml { get; set; }

        [DataMember]
        public string PhotosXml { get; set; }

        private string market;
        [DataMember]
        public string Market
        {
            get
            {
                return this.market;
            }
            set
            {
                this.market = value;
                if (this.market != null)
                {
                    this.market = this.market.Trim();
                }
            }
        }

        private string division;
        [DataMember]
        public string Division
        {
            get
            {
                return this.division;
            }
            set
            {
                this.division = value;
                if (this.division != null)
                {
                    this.division = this.division.Trim();
                }
            }
        }



        public bool Equals(OrderInfo other)
        {
            bool result = ((this.ID == other.ID) &&
                     (this.Customer.ToString() == other.Customer.ToString()) &&
                     (this.PONumber == other.PONumber) &&
                     (this.StoreNumber == other.StoreNumber) &&
                     (this.IsScheduled == other.IsScheduled) &&
                     (this.ScheduledDate.ToLongDateString() == other.ScheduledDate.ToLongDateString()) &&
                     (this.Market == other.Market) &&
                     (this.Division == other.Division));

            return result;
        }


        // this property is not a part of the service contract
        public NoteInfo[] Notes
        {
            get
            {
                if (string.IsNullOrEmpty(NotesXml) == true)
                {
                    return new NoteInfo[0];
                }
                else
                {
                    return NoteInfo.ParseNotesXml( NotesXml );
                }
            }
            set
            {
                if (value.Length == 0)
                {
                    NotesXml = null;
                }
                else
                {
                    NotesXml = NoteInfo.BuildNotesXml( value );
                }
            }
        }
		
		public void AddPhoto( PhotoInfo photo )
		{
			//form a mutable list from the existing array
			List<PhotoInfo> photos = new List<PhotoInfo>( Photos );
			
			// add the item to the list
			photos.Add( photo );
			
			// update the XML since it is the base representation for the collection
			Photos = photos.ToArray();
		}

        // this property is not a part of the service contract
        public PhotoInfo[] Photos
        {
            get
            {
                if (string.IsNullOrEmpty(PhotosXml) == true)
                {
                    return new PhotoInfo[0];
                }
                else
                {
                    return PhotoInfo.ParsePhotosXml(PhotosXml);
                }
            }
            set
            {
                if (value.Length == 0)
                {
                    PhotosXml = null;
                }
                else
                {
                    PhotosXml = PhotoInfo.BuildPhotosXml(value);
                }
            }
        }

        public bool IsScheduled
        {
            get
            {
                return ScheduledDate.Date != DateTime.MaxValue.Date;
            }
        }

        public bool HasDiagram
        {
            get
            {
                return (string.IsNullOrEmpty(DiagramNumber) == false );
            }
        }

        public string DebugText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("-------------------------------------------------------");
                sb.AppendFormat("Customer:      {0}\r\n", Customer.ToString());
                sb.AppendFormat("ID:            {0}\r\n", ID);
                sb.AppendFormat("Number:        {0}\r\n", PONumber);
                sb.AppendFormat("Division:      {0}\r\n", Division);
                sb.AppendFormat("Market:        {0}\r\n", Market);
                sb.AppendFormat("Store Number:  {0}\r\n", StoreNumber);
                sb.AppendFormat("Diagram Number:{0}\r\n", DiagramNumber);
                sb.AppendFormat("Scheduled for: {0}\r\n", ScheduledDate == DateTime.MaxValue ? "Not Scheduled" : ScheduledDate.ToLongDateString());

                if ( Notes.Length > 0 )
                {
                    sb.AppendLine("Notes ------------------------------------------");
                    foreach (NoteInfo note in Notes as IEnumerable<NoteInfo>)
                    {
                        sb.Append(note.DebugText);
                        sb.AppendLine("---");
                    }
                    sb.AppendLine();
                }

                if ( Photos.Length > 0 )
                {
                    sb.AppendLine("Photos ------------------------------------------");
                    foreach (PhotoInfo photo in Photos as IEnumerable<PhotoInfo>)
                    {
                        sb.Append(photo.DebugText);
                        sb.AppendLine("---");
                    }
                    sb.AppendLine();
                }

                return sb.ToString();
            }
        }

        private const string idTag = "ID";
        private const string poNumberTag = "PONumber";
        private const string storeNumberTag = "StoreNumber";
        private const string scheduleDateTag = "ScheduleDate";
        private const string marketTag = "Market";
        private const string divisionTag = "Division";
        private const string diagramNumberTag = "DiagramNumber";
        private const string customerTag = "Customer";
        public void WriteOrderDetailsXml( XmlTextWriter writer )
        {
            writer.WriteElementString( idTag, this.ID.ToString() );
            CustomerInfo.WriteCustomerXml(writer, this.Customer);
            writer.WriteElementString(poNumberTag, PONumber);
            writer.WriteElementString(storeNumberTag, StoreNumber);
            writer.WriteElementString(marketTag, Market.Trim());
            writer.WriteElementString(divisionTag, Division.Trim());
            writer.WriteElementString(diagramNumberTag, DiagramNumber);
            writer.WriteElementString(scheduleDateTag, ScheduledDate.ToShortDateString());
        }

        public void ParseOrderDetailsXml(XmlElement orderElement)
        {
            this.ID = int.Parse(orderElement.GetElementsByTagName(idTag)[0].InnerText);
            this.PONumber = orderElement.GetElementsByTagName(poNumberTag)[0].InnerText;
            this.StoreNumber = orderElement.GetElementsByTagName(storeNumberTag)[0].InnerText;
            this.Market = orderElement.GetElementsByTagName(marketTag)[0].InnerText;
            this.Division = orderElement.GetElementsByTagName(divisionTag)[0].InnerText;
            this.DiagramNumber = orderElement.GetElementsByTagName(diagramNumberTag)[0].InnerText;
            this.ScheduledDate = DateTime.Parse( orderElement.GetElementsByTagName(scheduleDateTag)[0].InnerText );
            this.Customer = CustomerInfo.ParseCustomer(  orderElement.GetElementsByTagName(customerTag)[0] as XmlElement );

            if ( string.IsNullOrEmpty(this.Division) == false )
            {
                this.Division = this.Division.Trim();
            }
            if (string.IsNullOrEmpty(this.Market) == false)
            {
                this.Market = this.Market.Trim();
            }

        }




        
    }

}
