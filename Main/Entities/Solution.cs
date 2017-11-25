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
    
    public partial class Solution
    {
        public Solution()
        {
            this.Vehicles = new HashSet<Vehicle>();
        }
    
        public int Id { get; set; }
        public int InstanceId { get; set; }
        public string Name { get; set; }
        public int Generation { get; set; }
        public Nullable<int> FatherId { get; set; }
        public Nullable<int> MotherId { get; set; }
        public bool Mutant { get; set; }
        public bool BestInGeneration { get; set; }
        public bool BestOfAll { get; set; }
        public bool LastGeneration { get; set; }
        public Nullable<long> TimeElapsedInMilliseconds { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int Fase { get; set; }
    
        public virtual Instance Instance { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
