using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.NodeStrategies
{
    public class NodeComparer
    {
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if(currentDecision.IsDecisionFinal()) return currentDecision;
            return comparison.AreNodeTypesEqual() 
                ? CompareResult.Same
                : CompareResult.Different;
        }
    }
}
