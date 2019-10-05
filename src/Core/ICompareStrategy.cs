namespace Egil.AngleSharp.Diffing.Core
{
    /// <summary>
    /// The compare strategy used by the <see cref="HtmlDifferenceEngine"/> 
    /// when comparing matched up nodes and attributes from the DOM-tree.
    /// </summary>
    public interface ICompareStrategy
    {
        /// <summary>
        /// Compares the control and test nodes in the <see cref="Comparison{TNode}"/> object and 
        /// tells the caller whether they are to be considered <see cref="CompareResult.Different"/> or 
        /// <see cref="CompareResult.Same"/>.
        /// </summary>
        /// <returns><see cref="CompareResult.Same"/> if the comparison is successful, <see cref="CompareResult.Different"/> otherwise.</returns>
        CompareResult Compare(in Comparison comparison);

        /// <summary>
        /// Compares the control and test attributes in the <see cref="AttributeComparison"/> object and 
        /// tells the caller whether they are to be considered <see cref="CompareResult.Different"/> or 
        /// <see cref="CompareResult.Same"/>.
        /// </summary>
        /// <returns><see cref="CompareResult.Same"/> if the comparison is successful, <see cref="CompareResult.Different"/> otherwise.</returns>
        CompareResult Compare(in AttributeComparison comparison);
    }
}
