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
    
    public partial class StoreType
    {
        public StoreType()
        {
            this.MaterialTypes = new HashSet<MaterialType>();
            this.Stores = new HashSet<Store>();
        }
    
        public int StoreTypeID { get; set; }
        public string StoreTypeName { get; set; }
        public string ImageName { get; set; }
    
        public virtual ICollection<MaterialType> MaterialTypes { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
