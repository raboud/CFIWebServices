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
    
    public partial class Order_Options_Detail
    {
        public int OrdeOptionslID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> OptionID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<int> SubContractorID { get; set; }
        public Nullable<decimal> UnitCost { get; set; }
        public Nullable<decimal> UnitRetail { get; set; }
        public Nullable<bool> SubContractorPaid { get; set; }
        public Nullable<double> SubContractorPay { get; set; }
        public int EntryMethodID { get; set; }
        public Nullable<bool> PrintOnInvoice { get; set; }
        public Nullable<bool> PrintOnWO { get; set; }
        public bool Deleted { get; set; }
        public bool Reviewed { get; set; }
        public Nullable<int> ReviewedBy { get; set; }
        public Nullable<System.DateTime> ReviewedDate { get; set; }
    
        public virtual Option Option { get; set; }
        public virtual Order Order { get; set; }
    }
}