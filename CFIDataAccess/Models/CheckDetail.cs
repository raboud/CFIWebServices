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
    
    public partial class CheckDetail
    {
        public int CheckDetailID { get; set; }
        public Nullable<int> CheckID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<decimal> Amount { get; set; }
    
        public virtual Check Check { get; set; }
        public virtual Order Order { get; set; }
    }
}
