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
    
    public partial class Employee
    {
        public Employee()
        {
            this.UserMarketDivisionAssignments = new HashSet<UserMarketDivisionAssignment>();
            this.UserPermissions = new HashSet<UserPermission>();
        }
    
        public int ID { get; set; }
        public int uid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomeNumber { get; set; }
        public string MobileNumber { get; set; }
        public string PagerNumber { get; set; }
        public string SSN { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> HireDate { get; set; }
        public Nullable<bool> ReceiveCallNotes { get; set; }
        public string SMTPEmail { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<UserMarketDivisionAssignment> UserMarketDivisionAssignments { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}
