//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFIDataAccess.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MeasureCompCustData
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string BusinessPhone { get; set; }
        public string CellPhone { get; set; }
        public string Pager { get; set; }
        public string CrossStreetDir1 { get; set; }
        public string CrossStreet1 { get; set; }
        public string CrossStreetDir2 { get; set; }
        public string CrossStreet2 { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
