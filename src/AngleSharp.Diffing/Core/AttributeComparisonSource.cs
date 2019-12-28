using System;
using System.Diagnostics.CodeAnalysis;

using AngleSharp.Diffing.Extensions;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents an attribute that can be a source in a comparison.
    /// </summary>
    public readonly struct AttributeComparisonSource : IEquatable<AttributeComparisonSource>, IComparisonSource
    {
        /// <summary>
        /// Gets the attribute attached to this source.
        /// </summary>
        public IAttr Attribute { get; }

        /// <summary>
        /// Gets the element source this attribute source is related to.
        /// </summary>
        public ComparisonSource ElementSource { get; }

        /// <summary>
        /// Gets the path to the attribute in the source node tree.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the source type, e.g. if it is a test or control source.
        /// </summary>
        public ComparisonSourceType SourceType { get; }

        /// <summary>
        /// Creates an instance of the <see cref="AttributeComparisonSource"/>.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="elementSource">The source of the element the attribute belongs to.</param>
        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Path should be in lower case")]
        public AttributeComparisonSource(string attributeName, in ComparisonSource elementSource)
        {
            if (string.IsNullOrEmpty(attributeName))
                throw new ArgumentNullException(nameof(attributeName));
            if (!elementSource.Node.TryGetAttr(attributeName, out var attribute))
                throw new ArgumentException("The comparison source does not contain an element or the specified attribute is missing on the element.", nameof(elementSource));

            Attribute = attribute;
            ElementSource = elementSource;
            SourceType = elementSource.SourceType;
            Path = $"{elementSource.Path}[{attribute.Name.ToLowerInvariant()}]";
        }

        #region Equals and HashCode
        /// <inheritdoc/>
        public bool Equals(AttributeComparisonSource other) => Object.ReferenceEquals(Attribute, other.Attribute) && Path.Equals(other.Path, StringComparison.Ordinal) && ElementSource.Equals(other.ElementSource);
        /// <inheritdoc/>
        public override int GetHashCode() => (Attribute, ElementSource).GetHashCode();
        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is AttributeComparisonSource other && Equals(other);
        /// <inheritdoc/>
        public static bool operator ==(AttributeComparisonSource left, AttributeComparisonSource right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(AttributeComparisonSource left, AttributeComparisonSource right) => !left.Equals(right);
        #endregion
    }
}
