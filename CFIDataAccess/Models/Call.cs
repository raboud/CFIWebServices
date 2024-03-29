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
    
    public partial class Call
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public short Location { get; set; }
        public short SpokeWith { get; set; }
        public string Other { get; set; }
        public string Comments { get; set; }
        public bool CustomerToCallBack { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public bool Schedule { get; set; }
        public Nullable<System.DateTime> ScheduledDate { get; set; }
        public Nullable<bool> ScheduledAM { get; set; }
        public bool Unscheduled { get; set; }
        public Nullable<short> uid { get; set; }
        public Nullable<bool> NeedLabor { get; set; }
        public Nullable<bool> NeedColor { get; set; }
        public Nullable<bool> NeedStyle { get; set; }
        public Nullable<bool> NeedPadding { get; set; }
        public Nullable<bool> NeedDrawings { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
