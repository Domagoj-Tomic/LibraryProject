//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryBackend
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserWorkshop
    {
        public int UserWorkshopID { get; set; }
        public int UserID { get; set; }
        public int WorkshopID { get; set; }
    
        public virtual User User { get; set; }
        public virtual Workshop Workshop { get; set; }
    }
}