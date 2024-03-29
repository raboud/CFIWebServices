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
    
    public partial class Customer
    {
        public Customer()
        {
            this.MeasureCompCustDatas = new HashSet<MeasureCompCustData>();
            this.Orders = new HashSet<Order>();
        }
    
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkNumber { get; set; }
        public string Directions { get; set; }
        public string Extension { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDateTime { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
    
        public virtual ICollection<MeasureCompCustData> MeasureCompCustDatas { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
