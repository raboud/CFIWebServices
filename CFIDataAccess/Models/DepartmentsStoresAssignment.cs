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
    
    public partial class DepartmentsStoresAssignment
    {
        public int ID { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string FaxNumber { get; set; }
        public string Notes { get; set; }
        public string AltFaxNumber { get; set; }
        public Nullable<int> FaxStatusReportTo { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Store Store { get; set; }
    }
}
