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
    
    public partial class BadgeEvent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BadgeEvent()
        {
            this.Positions = new HashSet<Position>();
        }
    
        public int BadgeID_Badge { get; set; }
        public int EventID_Event { get; set; }
    
        public virtual Event Event { get; set; }
        public virtual Badge Badge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Position> Positions { get; set; }
    }
}
