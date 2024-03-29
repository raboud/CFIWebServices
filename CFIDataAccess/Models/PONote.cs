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
    
    public partial class PONote
    {
        public PONote()
        {
            this.ActivityLists = new HashSet<ActivityList>();
        }
    
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int NoteTypeID { get; set; }
        public int SpokeWithID { get; set; }
        public string ContactName { get; set; }
        public System.DateTime DateTimeEntered { get; set; }
        public string NoteText { get; set; }
        public Nullable<int> EnteredByUserID { get; set; }
        public bool CustomerToCallBack { get; set; }
        public bool Scheduled { get; set; }
        public bool UnScheduled { get; set; }
        public Nullable<bool> ScheduledAM { get; set; }
        public Nullable<System.DateTime> ScheduledDate { get; set; }
        public bool Deleted { get; set; }
        public bool SentViaXML { get; set; }
        public Nullable<System.DateTime> DateTimeSent { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual ICollection<ActivityList> ActivityLists { get; set; }
    }
}
