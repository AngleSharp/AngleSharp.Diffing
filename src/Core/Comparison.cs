using System;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represent a comparison between two nodes.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public readonly struct Comparison : IEquatable<Comparison>
    {
        public ComparisonSource Control { get; }

        public ComparisonSource Test { get; }

        public Comparison(in ComparisonSource control, in ComparisonSource test)
        {
            Control = control;
            Test = test;
        }

        #region Equals and HashCode
        public bool Equals(Comparison other) => Control == other.Control && Test == other.Test;
        public override bool Equals(object obj) => obj is Comparison other && Equals(other);
        public override int GetHashCode() => (Control, Test).GetHashCode();
        public static bool operator ==(Comparison left, Comparison right) => left.Equals(right);
        public static bool operator !=(Comparison left, Comparison right) => !(left == right);
        #endregion
    }
}
