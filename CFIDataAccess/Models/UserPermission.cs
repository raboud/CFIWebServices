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
    
    public partial class UserPermission
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> Market { get; set; }
        public Nullable<int> Division { get; set; }
        public int PermissionID { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
