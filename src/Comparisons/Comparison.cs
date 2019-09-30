using System;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    /// <summary>
    /// Represent a comparison between two nodes.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public readonly struct Comparison<TNode> : IEquatable<Comparison<TNode>>, IComparison<TNode>
        where TNode : INode
    {
        public IComparisonSource<TNode> Control { get; }
        public IComparisonSource<TNode> Test { get; }

        public Comparison(in IComparisonSource<TNode> control, in IComparisonSource<TNode> test)
        {
            Control = control;
            Test = test;
        }

        #region Equals and HashCode
        public bool Equals(Comparison<TNode> other) => Control == other.Control && Test == other.Test;
        public override bool Equals(object obj) => obj is Comparison<TNode> other && Equals(other);
        public override int GetHashCode() => (Control, Test).GetHashCode();
        public static bool operator ==(Comparison<TNode> left, Comparison<TNode> right) => left.Equals(right);
        public static bool operator !=(Comparison<TNode> left, Comparison<TNode> right) => !(left == right);
        #endregion
    }
}
