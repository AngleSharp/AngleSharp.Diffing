using System;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    public readonly struct ComparisonSource : IEquatable<ComparisonSource>
    {
        public INode Node { get; }
        public int Index { get; }

        public ComparisonSource(INode node, int index)
        {
            Node = node;
            Index = index;
        }

        #region Equals and HashCode
        public bool Equals(ComparisonSource other) => Node == other.Node && Index == other.Index;
        public override int GetHashCode() => (Node, Index).GetHashCode();
        public override bool Equals(object obj) => obj is ComparisonSource other && Equals(other);
        public static bool operator ==(ComparisonSource left, ComparisonSource right) => left.Equals(right);
        public static bool operator !=(ComparisonSource left, ComparisonSource right) => !(left == right);
        #endregion
    }
}
