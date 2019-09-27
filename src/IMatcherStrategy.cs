using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    /// <summary>
    /// The node and attribute matching strategy used by the <see cref="HtmlDifferenceEngine"/> 
    /// when matching up nodes and attributes for comparison.
    /// </summary>
    public interface IMatcherStrategy
    {        
        /// <summary>
        /// Matches up the control nodes with test nodes in the two input lists. The matched control and test nodes will be compared to each other.
        /// Any unmatched control or test nodes will be reported as either <see cref="DiffResult.Missing"/> or <see cref="DiffResult.Unexpected"/>,
        /// depending on whether they are control or test nodes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IComparison<INode>> MatchNodes(IReadOnlyList<IComparisonSource<INode>> controlNodes, IReadOnlyList<IComparisonSource<INode>> testNodes);

        /// <summary>
        /// Matches up the control attributes with test attributes in the two input lists. The matched control and test attributes will be compared to each other.
        /// Any unmatched control or test attributes will be reported as either <see cref="DiffResult.Missing"/> or <see cref="DiffResult.Unexpected"/>,
        /// depending on whether they are control or test attributes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAttributeComparison> MatchAttributes(IReadOnlyList<IAttributeComparisonSource> controlAttributes, IReadOnlyList<IAttributeComparisonSource> testAttributes);
    }
}
