using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    /// <summary>
    /// The filter strategy used by the <see cref="HtmlDifferenceEngine"/> 
    /// when filtering out unwanted nodes and attributes from the DOM-tree before comparison.
    /// </summary>
    public interface IFilterStrategy
    {
        /// <summary>
        /// Decides whether a node should be part of the comparison.
        /// </summary>
        /// <param name="comparisonSource">A comparison source for the node</param>
        /// <returns>true if the node should be part of the comparison, false if the node should be filtered out.</returns>
        bool NodeFilter(in IComparisonSource<INode> comparisonSource);

        /// <summary>
        /// Decides whether an attribute should be part of the comparison.
        /// </summary>
        /// <param name="comparisonSource">A comparison source for the attribute</param>
        /// <returns>true if the attribute should be part of the comparison, false if the attribute should be filtered out.</returns>
        bool AttributeFilter(in IAttributeComparisonSource attributeComparisonSource);
    }
}
