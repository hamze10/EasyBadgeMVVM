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
    
    public partial class Position
    {
        public int BadgeEventBadgeID_Badge { get; set; }
        public int BadgeEventEventID_Event { get; set; }
        public int FieldID_Field { get; set; }
        public double Position_X { get; set; }
        public double Position_Y { get; set; }
    
        public virtual Field Field { get; set; }
        public virtual BadgeEvent BadgeEvent { get; set; }
    }
}