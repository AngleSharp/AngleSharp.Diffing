using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;
using Egil.AngleSharp.Diffing.Comparers.Chainable;

namespace Egil.AngleSharp.Diffing.Comparers
{
    public class DiffIgnoreAttributeCompareStrategy : IChainableNodeCompareStrategy<IElement>
    {
        public CompareResult Compare(IComparison<IElement> comparison, CompareResult currentDecision)
        {
            if (comparison is null) throw new ArgumentNullException(nameof(comparison));

            var ignoreAttr = comparison.Control.Node.Attributes["diff:ignore"];

            if (ignoreAttr is { } && ignoreAttr.IsEmptyOrEquals("TRUE"))
                return CompareResult.SameAndBreak;
            else 
                return currentDecision;
        }
    }
}
