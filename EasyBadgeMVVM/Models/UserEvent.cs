//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyBadgeMVVM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserEvent
    {
        public int UserID_User { get; set; }
        public int EventID_Event { get; set; }
        public int FieldUserID_FieldUser1 { get; set; }
    
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
        public virtual FieldUser FieldUser { get; set; }
    }
}