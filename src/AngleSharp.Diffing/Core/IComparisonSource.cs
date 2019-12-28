namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a node or attribute that can be one part of a comparison.
    /// </summary>
    public interface IComparisonSource
    {
        /// <summary>
        /// Gets the path to the node or attribute. The format is similar to a
        /// CSS selector, where the <c>(n)</c> in <c>node(n)</c> represents the items
        /// position in the DOM tree, relative to other items.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets whether the source is from the control or test DOM tree.
        /// </summary>
        ComparisonSourceType SourceType { get; }
    }
}