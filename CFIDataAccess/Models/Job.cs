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
    
    public partial class Job
    {
        public Job()
        {
            this.JobDetails = new HashSet<JobDetail>();
        }
    
        public Nullable<int> OrderID { get; set; }
        public int JobID { get; set; }
        public Nullable<System.DateTime> DateScheduled { get; set; }
        public Nullable<int> SubContractorID { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual SubContractor SubContractor { get; set; }
        public virtual ICollection<JobDetail> JobDetails { get; set; }
    }
}
