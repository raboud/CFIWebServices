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
    
    public partial class OrderBasicLaborDetailsDeleted
    {
        public int ID { get; set; }
        public int OriginalID { get; set; }
        public int OrderID { get; set; }
        public int BasicLaborID { get; set; }
        public Nullable<double> InstallQuantity { get; set; }
        public Nullable<decimal> UnitCost { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> UnitRetail { get; set; }
        public bool PrintOnInvoice { get; set; }
        public bool PrintOnWO { get; set; }
        public Nullable<int> ServiceLineNumber { get; set; }
        public int EntryMethodID { get; set; }
        public bool Deleted { get; set; }
    }
}
