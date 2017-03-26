using Main.BrkgaTop;
using Main.Model;
using System;

namespace Main.GuidedLocalSearchHeuristics
{
    public class ReplaceHeuristic : ILocalSearchHeuristic
    {
        // Necesito: 
        // Los que estan afuera (ordenados por que tan copados son)
        // Los que estan adentro (ordenados por que tan copados son)
        // Poder busar posicion ideal
        // Hacer el swap. El swap debe reflejarse en el codigo que. Es decir que cuando decodifique, debe quedar la solucion mejorada
        public void ApplyHeuristic(ref EncodedSolution encodedSolution)
        {
            throw new NotImplementedException();
        }
    }
}
