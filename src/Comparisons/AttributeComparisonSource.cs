using System;
using AngleSharp.Dom;
using System.Diagnostics.CodeAnalysis;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    public readonly struct AttributeComparisonSource : IEquatable<AttributeComparisonSource>, IAttributeComparisonSource
    {
        public IAttr Attribute { get; }

        public IComparisonSource<IElement> ElementSource { get; }

        public string Path { get; }

        public ComparisonSourceType SourceType { get; }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Path should be in lower case")]
        public AttributeComparisonSource(IAttr attribute, in IComparisonSource<IElement> elementSource)
        {
            if (attribute is null) throw new ArgumentNullException(nameof(attribute));
            if (elementSource is null) throw new ArgumentNullException(nameof(elementSource));

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
        public static bool operator !=(AttributeComparisonSource left, AttributeComparisonSource right) => !(left == right);
        #endregion
    }
}
