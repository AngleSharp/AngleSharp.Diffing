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
    /// <summary>
    /// Node filter that removes #comment nodes from the comparison.
    /// </summary>
    public class RemoveCommentsNodeFilter : IChainableNodeFilterStrategy<IComment>
    {
        public bool Filter(IComparisonSource<IComment> comparisonSource, bool currentDecision) => false;
    }
}