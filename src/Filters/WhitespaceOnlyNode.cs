using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;
using Egil.AngleSharp.Diffing.Filters.Chainable;

namespace Egil.AngleSharp.Diffing.Filters
{
    public class WhitespaceOnlyNode : IChainableNodeFilterStrategy<IText>
    {
        public bool Filter(IComparisonSource<IText> comparisonSource, bool currentDecision)
        {
            if(comparisonSource is null) throw new ArgumentNullException(nameof(comparisonSource));

            var result = currentDecision;
            
            if (result)
            {
                result = !string.IsNullOrWhiteSpace(comparisonSource.Node.Data);
            }

            return result;
        }
    }
}
