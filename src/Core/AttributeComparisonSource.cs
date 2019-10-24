using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Egil.AngleSharp.Diffing.Core
{
    public readonly struct AttributeComparisonSource : IEquatable<AttributeComparisonSource>, IComparisonSource
    {
        public IAttr Attribute { get; }

        public ComparisonSource ElementSource { get; }

        public string Path { get; }

        public ComparisonSourceType SourceType { get; }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Path should be in lower case")]
        public AttributeComparisonSource(string attributeName, in ComparisonSource elementSource)
        {
            if (string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException(nameof(attributeName));
            if (!elementSource.Node.TryGetAttr(attributeName, out var attribute))
                throw new ArgumentException("The comparison source does not contain an element or the specified attribute is missing on the element.", nameof(elementSource));

            Attribute = attribute;
            ElementSource = elementSource;
            SourceType = elementSource.SourceType;
            Path = $"{elementSource.Path}[{attribute.Name.ToLowerInvariant()}]";
        }

        #region Equals and HashCode
        public bool Equals(AttributeComparisonSource other) => Attribute == other.Attribute && ElementSource == other.ElementSource && Path.Equals(other.Path, StringComparison.Ordinal);
        public override int GetHashCode() => (Attribute, ElementSource).GetHashCode();
        public override bool Equals(object obj) => obj is AttributeComparisonSource other && Equals(other);
        public static bool operator ==(AttributeComparisonSource left, AttributeComparisonSource right) => left.Equals(right);
        public static bool operator !=(AttributeComparisonSource left, AttributeComparisonSource right) => !left.Equals(right);
        #endregion
    }
}
