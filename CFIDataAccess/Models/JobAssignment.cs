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
    
    public partial class JobAssignment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> SubContractorId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }
}