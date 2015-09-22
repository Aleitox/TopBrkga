namespace Main.Brkga
{
    public interface IProblemManager
    {
        void Decode(IPopulation population);
        bool StoppingRuleFulfilled { get; }
    }
}
