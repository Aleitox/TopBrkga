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
    
    public partial class ExternalSolution
    {
        public int Id { get; set; }
        public Nullable<int> InstanceId { get; set; }
        public string InstanceName { get; set; }
        public Nullable<int> AHS_Gen_Tabu_Penalty_Min { get; set; }
        public Nullable<int> AHS_Gen_Tabu_Penalty_Max { get; set; }
        public Nullable<int> AHS_Gen_Tabu_Feasible_Min { get; set; }
        public Nullable<int> AHS_Gen_Tabu_Feasible_Max { get; set; }
        public Nullable<int> AHS_Fast_Vns_Feasible_Min { get; set; }
        public Nullable<int> AHS_Fast_Vns_Feasible_Max { get; set; }
        public Nullable<int> AHS_Slow_Vns_Feasible_Min { get; set; }
        public Nullable<int> AHS_Slow_Vns_Feasible_Max { get; set; }
        public Nullable<int> TMH { get; set; }
        public Nullable<int> CGH { get; set; }
    }
}
