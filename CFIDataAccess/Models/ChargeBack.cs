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
    
    public partial class ChargeBack
    {
        public ChargeBack()
        {
            this.CheckCBDetails = new HashSet<CheckCBDetail>();
        }
    
        public int ChargeBackID { get; set; }
        public decimal Amount { get; set; }
        public int SubcontractorID { get; set; }
        public decimal AmountToSub { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public System.DateTime IssueDate { get; set; }
        public string OriginalPO { get; set; }
        public Nullable<bool> CostAdjustment { get; set; }
        public Nullable<int> OrderID { get; set; }
        public int ApprovalNumber { get; set; }
        public bool Approved { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual ICollection<CheckCBDetail> CheckCBDetails { get; set; }
    }
}
