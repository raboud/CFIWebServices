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
    
    public partial class Department
    {
        public Department()
        {
            this.DepartmentsContactTitles = new HashSet<DepartmentsContactTitle>();
            this.DepartmentsStoresAssignments = new HashSet<DepartmentsStoresAssignment>();
            this.StoreContacts = new HashSet<StoreContact>();
        }
    
        public int ID { get; set; }
        public string DepartmentName { get; set; }
    
        public virtual ICollection<DepartmentsContactTitle> DepartmentsContactTitles { get; set; }
        public virtual ICollection<DepartmentsStoresAssignment> DepartmentsStoresAssignments { get; set; }
        public virtual ICollection<StoreContact> StoreContacts { get; set; }
    }
}