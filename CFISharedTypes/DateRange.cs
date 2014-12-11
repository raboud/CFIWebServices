using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CFI
{
    [DataContract]
    public class DateRange
    {
        [DataMember]
        public DateTime Start = DateTime.MaxValue;
        
        [DataMember]
        public DateTime End = DateTime.MaxValue;
    }
}
