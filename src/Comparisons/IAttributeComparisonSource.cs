using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    /// <summary>
    /// A input source for an attribute comparison. 
    /// </summary>
    public interface IAttributeComparisonSource
    {
        /// <summary>
        /// Gets the attribute which should take part in a comparison.
        /// </summary>
        IAttr Attribute { get; }

        /// <summary>
        /// Gets the source of the element which the attribute belongs to.
        /// </summary>
        IComparisonSource<IElement> ElementSource { get; }

        /// <summary>
        /// Gets an CSS-selector like representation of the path to the attribute in the DOM-tree.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the source type, i.e. if this is a <see cref="ComparisonSourceType.Control"/> or <see cref="ComparisonSourceType.Test"/> source.
        /// </summary>
        ComparisonSourceType SourceType { get; }
    }
}
