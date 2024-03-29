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
    
    public partial class Order
    {
        public Order()
        {
            this.ActionReports = new HashSet<ActionReport>();
            this.ActivityLists = new HashSet<ActivityList>();
            this.Calls = new HashSet<Call>();
            this.ChargeBacks = new HashSet<ChargeBack>();
            this.CheckDetails = new HashSet<CheckDetail>();
            this.Jobs = new HashSet<Job>();
            this.MeasureCompCalcDatas = new HashSet<MeasureCompCalcData>();
            this.MeasureCompOrderDatas = new HashSet<MeasureCompOrderData>();
            this.Order_Options_Details = new HashSet<Order_Options_Detail>();
            this.OrderBasicLaborDetails = new HashSet<OrderBasicLaborDetail>();
            this.OrderCustomDetails = new HashSet<OrderCustomDetail>();
            this.OrderDiagrams = new HashSet<OrderDiagram>();
            this.OrderExtras = new HashSet<OrderExtra>();
            this.OrderRegMerchandiseDetails = new HashSet<OrderRegMerchandiseDetail>();
            this.OrderSOMerchandiseDetails = new HashSet<OrderSOMerchandiseDetail>();
            this.PONotes = new HashSet<PONote>();
            this.POPhotos = new HashSet<POPhoto>();
        }
    
        public int OrderID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> ScheduleStartDate { get; set; }
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
        public string SPNNotes { get; set; }
        public Nullable<int> XMLOrderCostAmount { get; set; }
        public bool Deleted { get; set; }
        public bool Reviewed { get; set; }
        public Nullable<int> ReviewedBy { get; set; }
        public Nullable<System.DateTime> ReviewedDate { get; set; }
        public Nullable<int> VendorID { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string CorrelationID { get; set; }
        public string KeyRecNumber { get; set; }
        public Nullable<System.DateTime> KeyRecDate { get; set; }
        public Nullable<System.DateTime> SvcCompleteSentDate { get; set; }
        public Nullable<System.DateTime> ScheduleEndDate { get; set; }
    
        public virtual ICollection<ActionReport> ActionReports { get; set; }
        public virtual ICollection<ActivityList> ActivityLists { get; set; }
        public virtual ICollection<Call> Calls { get; set; }
        public virtual ICollection<ChargeBack> ChargeBacks { get; set; }
        public virtual ICollection<CheckDetail> CheckDetails { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<MeasureCompCalcData> MeasureCompCalcDatas { get; set; }
        public virtual ICollection<MeasureCompOrderData> MeasureCompOrderDatas { get; set; }
        public virtual ICollection<Order_Options_Detail> Order_Options_Details { get; set; }
        public virtual ICollection<OrderBasicLaborDetail> OrderBasicLaborDetails { get; set; }
        public virtual ICollection<OrderCustomDetail> OrderCustomDetails { get; set; }
        public virtual ICollection<OrderDiagram> OrderDiagrams { get; set; }
        public virtual ICollection<OrderExtra> OrderExtras { get; set; }
        public virtual ICollection<OrderRegMerchandiseDetail> OrderRegMerchandiseDetails { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<OrderSOMerchandiseDetail> OrderSOMerchandiseDetails { get; set; }
        public virtual ICollection<PONote> PONotes { get; set; }
        public virtual ICollection<POPhoto> POPhotos { get; set; }
    }
}
