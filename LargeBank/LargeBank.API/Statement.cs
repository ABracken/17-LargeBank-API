//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LargeBank.API
{
    using System;
    using System.Collections.Generic;
    
    public partial class Statement
    {
        public int StatementID { get; set; }
        public int AccountID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
