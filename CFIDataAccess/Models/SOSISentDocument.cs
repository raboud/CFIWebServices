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
    
    public partial class SOSISentDocument
    {
        public int ID { get; set; }
        public int DocTypeID { get; set; }
        public string MatchField1 { get; set; }
        public string MatchField2 { get; set; }
        public string MatchField3 { get; set; }
        public bool Acknowledged { get; set; }
        public string DocumentFileName { get; set; }
    }
}
