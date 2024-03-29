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
    
    public partial class StoreContact
    {
        public int ID { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> TitleID { get; set; }
        public string PhoneNumber { get; set; }
        public string AltPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<bool> EmailStatusReport { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Store Store { get; set; }
    }
}
