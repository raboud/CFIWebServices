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
    
    public partial class BasicLabor
    {
        public BasicLabor()
        {
            this.OrderBasicLaborDetails = new HashSet<OrderBasicLaborDetail>();
        }
    
        public Nullable<int> UnitOfMeasureID { get; set; }
        public int BasicLaborID { get; set; }
        public string LaborDescription { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> UnitCost { get; set; }
        public Nullable<int> MaterialTypeID { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public Nullable<bool> IsPadding { get; set; }
        public Nullable<int> YardsPerRoll { get; set; }
        public int MaterialCatagoryID { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> MaterialCost { get; set; }
        public Nullable<bool> PrintOnWO { get; set; }
    
        public virtual ICollection<OrderBasicLaborDetail> OrderBasicLaborDetails { get; set; }
    }
}
