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
    
    public partial class SPNImportedPOs
    {
        public int ID { get; set; }
        public string PONumber { get; set; }
        public int StoreID { get; set; }
        public int POStatusID { get; set; }
        public string XMLFilePath { get; set; }
        public System.DateTime DateImported { get; set; }
    }
}
