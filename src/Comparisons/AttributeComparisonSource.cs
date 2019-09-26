using System;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    public readonly struct AttributeComparisonSource : IEquatable<AttributeComparisonSource>, IAttributeComparisonSource
    {
        public IAttr Attribute { get; }

        public IComparisonSource<IElement> ElementSource { get; }

        public AttributeComparisonSource(IAttr attribute, in IComparisonSource<IElement> elementSource)
        {
            Attribute = attribute;
            ElementSource = elementSource;
        }

        #region Equals and HashCode
        public bool Equals(AttributeComparisonSource other) => Attribute == other.Attribute && ElementSource == other.ElementSource;
        public override int GetHashCode() => (Attribute, ElementSource).GetHashCode();
        public override bool Equals(object obj) => obj is AttributeComparisonSource other && Equals(other);
        public static bool operator ==(AttributeComparisonSource left, AttributeComparisonSource right) => left.Equals(right);
        public static bool operator !=(AttributeComparisonSource left, AttributeComparisonSource right) => !(left == right);
        #endregion
    }
}
