using System;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    public readonly struct AttributeComparison : IEquatable<AttributeComparison>, IAttributeComparison
    {
        public IAttributeComparisonSource Control { get; }

        public IAttributeComparisonSource Test { get; }

        public AttributeComparison(IAttributeComparisonSource control, IAttributeComparisonSource test)
        {
            Control = control;
            Test = test;
        }

        #region Equals and HashCode
        public bool Equals(AttributeComparison other) => Control == other.Control && Test == other.Test;
        public override bool Equals(object obj) => obj is AttributeComparison other && Equals(other);
        public override int GetHashCode() => (Control, Test).GetHashCode();
        public static bool operator ==(AttributeComparison left, AttributeComparison right) => left.Equals(right);
        public static bool operator !=(AttributeComparison left, AttributeComparison right) => !(left == right);
        #endregion
    }
}
