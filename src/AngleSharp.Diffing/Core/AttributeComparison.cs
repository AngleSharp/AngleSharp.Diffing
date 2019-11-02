using AngleSharp.Dom;
using System;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// A match between two attributes that should be compared.
    /// </summary>
    public readonly struct AttributeComparison : IEquatable<AttributeComparison>
    {
        /// <summary>
        /// Gets the control attribute which the <see cref="Test"/> attribute is supposed to match.
        /// </summary>
        public AttributeComparisonSource Control { get; }

        /// <summary>
        /// Gets the test attribute which should be compared to the <see cref="Control"/> attribute.
        /// </summary>
        public AttributeComparisonSource Test { get; }

        /// <summary>
        /// Create a attribute comparison match.
        /// </summary>
        /// <param name="control">The attribute control source</param>
        /// <param name="test">The attribute test source</param>
        public AttributeComparison(in AttributeComparisonSource control, in AttributeComparisonSource test)
        {
            Control = control;
            Test = test;
        }

        /// <summary>
        /// Returns the control and test elements which the control and test attributes belongs to.
        /// </summary>
        public (IElement ControlElement, IElement TestElement) GetAttributeElements()
            => ((IElement)Control.ElementSource.Node, (IElement)Test.ElementSource.Node);

        #region Equals and HashCode
        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(AttributeComparison other) => Control.Equals(other.Control) && Test.Equals(other.Test);
        public override bool Equals(object? obj) => obj is AttributeComparison other && Equals(other);
        public override int GetHashCode() => (Control, Test).GetHashCode();
        public static bool operator ==(AttributeComparison left, AttributeComparison right) => left.Equals(right);
        public static bool operator !=(AttributeComparison left, AttributeComparison right) => !left.Equals(right);
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion
    }
}
