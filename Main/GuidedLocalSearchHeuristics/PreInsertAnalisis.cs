
namespace Main.GuidedLocalSearchHeuristics
{
    public class PreInsertAnalisis
    {
        public decimal NewTimeBudgetAvg { get; set; }

        public decimal CurrentTimeBudgetAvg { get; set; }

        public bool CanBeInserted { get; set; }

        public int BestInsertPosition { get; set; }
    }
}
