using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    /// <summary>
    /// The compare strategy used by the <see cref="HtmlDifferenceEngine"/> 
    /// when comparing matched up nodes and attributes from the DOM-tree.
    /// </summary>
    public interface ICompareStrategy
    {
        /// <summary>
        /// Compares the control and test nodes in the <see cref="IComparison{TNode}"/> object and 
        /// tells the caller whether they are to be considered <see cref="CompareResult.Different"/> or 
        /// <see cref="CompareResult.Same"/>.
        /// </summary>
        /// <returns><see cref="CompareResult.Same"/> if the comparison is successful, <see cref="CompareResult.Different"/> otherwise.</returns>
        CompareResult Compare<TNode>(in IComparison<TNode> comparison) where TNode : INode;

        /// <summary>
        /// Compares the control and test attributes in the <see cref="IAttributeComparison"/> object and 
        /// tells the caller whether they are to be considered <see cref="CompareResult.Different"/> or 
        /// <see cref="CompareResult.Same"/>.
        /// </summary>
        /// <returns><see cref="CompareResult.Same"/> if the comparison is successful, <see cref="CompareResult.Different"/> otherwise.</returns>
        CompareResult Compare(in IAttributeComparison comparison);
    }
}
