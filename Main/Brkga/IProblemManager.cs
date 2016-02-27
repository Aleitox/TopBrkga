namespace Main.Brkga
{
    public interface IProblemManager
    {
        bool StoppingRuleFulfilled { get; }

        Population Population { get; set; }

        int IterationNumber { get; set; }
    }

    public class ProblemManager : IProblemManager
    {
        public bool StoppingRuleFulfilled { get { return IterationNumber >= 100; } }

        public Population Population { get; set; }

        public int IterationNumber { get; set; }
    }
}
