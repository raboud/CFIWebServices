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
    
    public partial class JobDetail
    {
        public int JobDetailID { get; set; }
        public Nullable<int> JobID { get; set; }
        public Nullable<int> DetailID { get; set; }
        public Nullable<int> DetailType { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
    
        public virtual Job Job { get; set; }
    }
}