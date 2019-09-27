using System;
using AngleSharp.Dom;
using System.Diagnostics.CodeAnalysis;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    public readonly struct ComparisonSource<TNode> : IEquatable<ComparisonSource<TNode>>, IComparisonSource<TNode> where TNode : INode
    {
        public TNode Node { get; }

        public int Index { get; }

        public string Path { get; }

        public ComparisonSourceType SourceType { get; }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Path should be in lower case")]
        public ComparisonSource(TNode node, int index, string path, ComparisonSourceType sourceType)
        {
            Node = node;
            Index = index;
            Path = string.IsNullOrEmpty(path) 
                ? $"{Node.NodeName.ToLowerInvariant()}({Index})" 
                : $"{path} > {Node.NodeName.ToLowerInvariant()}({Index})";

            SourceType = sourceType;
        }

        #region Equals and HashCode
        public bool Equals(ComparisonSource<TNode> other) => Node.Equals(other.Node) && Index == other.Index && Path.Equals(other.Path, StringComparison.Ordinal) && SourceType == other.SourceType;
        public override int GetHashCode() => (Node, Index, Path, SourceType).GetHashCode();
        public override bool Equals(object obj) => obj is ComparisonSource<TNode> other && Equals(other);
        public static bool operator ==(ComparisonSource<TNode> left, ComparisonSource<TNode> right) => left.Equals(right);
        public static bool operator !=(ComparisonSource<TNode> left, ComparisonSource<TNode> right) => !(left == right);
        #endregion
    }
}
