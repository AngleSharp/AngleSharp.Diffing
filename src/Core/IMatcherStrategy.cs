using System.Collections.Generic;

namespace Egil.AngleSharp.Diffing.Core
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
        IEnumerable<Comparison> MatchNodes(DiffContext context, SourceCollection controlSources, SourceCollection testSources);

        /// <summary>
        /// Matches up the control attributes with test attributes in the two input lists. The matched control and test attributes will be compared to each other.
        /// Any unmatched control or test attributes will be reported as either <see cref="DiffResult.Missing"/> or <see cref="DiffResult.Unexpected"/>,
        /// depending on whether they are control or test attributes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AttributeComparison> MatchAttributes(DiffContext context, SourceMap controlAttrSources, SourceMap testAttrSources);
    }
}
