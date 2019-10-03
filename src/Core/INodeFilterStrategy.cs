namespace Egil.AngleSharp.Diffing.Core
{
    public interface INodeFilterStrategy
    {
        /// <summary>
        /// Decides whether a node should be part of the comparison.
        /// </summary>
        /// <param name="comparisonSource">A comparison source for the node</param>
        /// <returns>true if the node should be part of the comparison, false if the node should be filtered out.</returns>
        bool NodeFilter(in ComparisonSource comparisonSource);
    }
}
