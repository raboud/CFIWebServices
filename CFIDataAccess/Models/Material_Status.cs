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
    
    public partial class Material_Status
    {
        public Material_Status()
        {
            this.OrderRegMerchandiseDetails = new HashSet<OrderRegMerchandiseDetail>();
            this.OrderSOMerchandiseDetails = new HashSet<OrderSOMerchandiseDetail>();
        }
    
        public int MatStatusID { get; set; }
        public string Status { get; set; }
        public bool Billable { get; set; }
        public bool Installable { get; set; }
    
        public virtual ICollection<OrderRegMerchandiseDetail> OrderRegMerchandiseDetails { get; set; }
        public virtual ICollection<OrderSOMerchandiseDetail> OrderSOMerchandiseDetails { get; set; }
    }
}
