using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    /// <summary>
    /// A input source for an node comparison. 
    /// </summary>
    /// <typeparam name="TNode">The type of the node</typeparam>
    public interface IComparisonSource<out TNode> where TNode : INode
    {
        /// <summary>
        /// Gets the node which should take part in a comparison.
        /// </summary>
        TNode Node { get; }

        /// <summary>
        /// Gets the index of the node in the DOM-tree, relative to its siblings.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets an CSS-selector like representation of the path to the node in the DOM-tree.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the source type, i.e. if this is a <see cref="ComparisonSourceType.Control"/> or <see cref="ComparisonSourceType.Test"/> source.
        /// </summary>
        ComparisonSourceType SourceType { get; }
    }
}
