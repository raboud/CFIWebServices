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
    
    public partial class ActivityList
    {
        public ActivityList()
        {
            this.ActivityDatas = new HashSet<ActivityData>();
            this.PONotes = new HashSet<PONote>();
        }
    
        public int ID { get; set; }
        public int ActivityTypeID { get; set; }
        public int OrderID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedByID { get; set; }
        public System.DateTime FollowUpDate { get; set; }
        public Nullable<int> ClosedByID { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
    
        public virtual ICollection<ActivityData> ActivityDatas { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<PONote> PONotes { get; set; }
    }
}
