using AngleSharp.Dom;
using System;

namespace Egil.AngleSharp.Diffing.Core
{
    public readonly struct AttributeComparison : IEquatable<AttributeComparison>
    {
        public AttributeComparisonSource Control { get; }

        public AttributeComparisonSource Test { get; }

        public AttributeComparison(in AttributeComparisonSource control, in AttributeComparisonSource test)
        {
            Control = control;
            Test = test;
        }
        
        public (IElement ControlElement, IElement TestElement) GetAttributeElements() 
            => ((IElement)Control.ElementSource.Node, (IElement)Test.ElementSource.Node);

        #region Equals and HashCode
        public bool Equals(AttributeComparison other) => Control == other.Control && Test == other.Test;
        public override bool Equals(object obj) => obj is AttributeComparison other && Equals(other);
        public override int GetHashCode() => (Control, Test).GetHashCode();
        public static bool operator ==(AttributeComparison left, AttributeComparison right) => left.Equals(right);
        public static bool operator !=(AttributeComparison left, AttributeComparison right) => !left.Equals(right);
        #endregion
    }
}
