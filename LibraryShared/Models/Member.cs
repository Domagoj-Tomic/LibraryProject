//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryShared.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Member
    {
        public int UserID { get; set; }
        public string Status { get; set; }
        public System.DateTime EnrolledFrom { get; set; }
        public Nullable<System.DateTime> EnrolledTo { get; set; }
    
        public virtual User User { get; set; }
    }
}
