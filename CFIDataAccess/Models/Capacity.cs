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
    
    public partial class Capacity
    {
        public int ID { get; set; }
        public int PoolID { get; set; }
        public System.DateTime CapacityDate { get; set; }
        public int CapAM { get; set; }
        public int CapPM { get; set; }
        public int CapNP { get; set; }
        public bool NonWorkDay { get; set; }
        public bool SendToHD { get; set; }
    }
}
