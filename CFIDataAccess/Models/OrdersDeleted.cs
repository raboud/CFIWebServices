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
    
    public partial class OrdersDeleted
    {
        public int ID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> ScheduleDate { get; set; }
        public Nullable<System.DateTime> BilledDate { get; set; }
        public bool Scheduled { get; set; }
        public bool Billed { get; set; }
        public bool Paid { get; set; }
        public bool Called { get; set; }
        public string InternalNotes { get; set; }
        public Nullable<decimal> CostAmount { get; set; }
        public Nullable<decimal> BilledAmount { get; set; }
        public int MaterialTypeID { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public Nullable<decimal> TripCharge { get; set; }
        public Nullable<bool> NoMinimum { get; set; }
        public Nullable<bool> ScheduledAM { get; set; }
        public bool Cancelled { get; set; }
        public bool Warrenty { get; set; }
        public int StoreID { get; set; }
        public bool SevenDay { get; set; }
        public string DrawingNumber { get; set; }
        public Nullable<System.DateTime> DrawingDate { get; set; }
        public bool CustomerToCall { get; set; }
        public bool Invoice { get; set; }
        public string OriginalPO { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> DateEntered { get; set; }
        public Nullable<int> EnteredBy { get; set; }
        public int EntryMethodID { get; set; }
        public Nullable<System.DateTime> FollowUpDate { get; set; }
        public string FollowUpAction { get; set; }
        public Nullable<int> ServiceLineNo { get; set; }
        public Nullable<System.DateTime> LastModifiedDateTime { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
    }
}
