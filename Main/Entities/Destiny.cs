//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Main.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Destiny
    {
        public Destiny()
        {
            this.VehiclesDestinies = new HashSet<VehiclesDestiny>();
        }
    
        public int Id { get; set; }
        public int InstanceId { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public int Profit { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<VehiclesDestiny> VehiclesDestinies { get; set; }
        public virtual Instance Instance { get; set; }
    }
}
