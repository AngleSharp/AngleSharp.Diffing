using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Extensions;

namespace Egil.AngleSharp.Diffing.Core
{
    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Path should be in lower case")]
    [DebuggerDisplay("{Index} : {Path}")]
    public readonly struct ComparisonSource : IEquatable<ComparisonSource>, IComparisonSource
    {
        private readonly int _hashCode;

        public INode Node { get; }

        public int Index { get; }

        public string Path { get; }

        public ComparisonSourceType SourceType { get; }

        public ComparisonSource(INode node, ComparisonSourceType sourceType)
        {
            Node = node;
            Index = GetNodeIndex(node);
            Path = CalculateNodePath(node, Index);
            SourceType = sourceType;
            _hashCode = (Node, Index, Path, SourceType).GetHashCode();
        }

        public ComparisonSource(INode node, int index, string path, ComparisonSourceType sourceType)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            Node = node;
            Index = index;
            Path = string.IsNullOrEmpty(path)
                ? $"{Node.NodeName.ToLowerInvariant()}({Index})"
                : $"{path} > {Node.NodeName.ToLowerInvariant()}({Index})";

            SourceType = sourceType;
            _hashCode = (Node, Index, Path, SourceType).GetHashCode();
        }

        private static int GetNodeIndex(INode node)
        {
            if (node.TryGetNodeIndex(out var index))
            {
                return index;
            }
            else
            {
                throw new ArgumentException("When the node does not have a parent its index cannot be calculated.", nameof(node));
            }
        }

        private static string CalculateNodePath(INode node, int index)
        {
            var path = $"{node.NodeName.ToLowerInvariant()}({index})";
            var parent = node.Parent;
            while (parent is { } && parent.TryGetNodeIndex(out var parentIndex))
            {
                path = $"{parent.NodeName.ToLowerInvariant()}({parentIndex}) > {path}";
                parent = parent.Parent;
            }
            return path;
        }

        #region Equals and HashCode
        public bool Equals(ComparisonSource other) => Node.Equals(other.Node) && Index == other.Index && Path.Equals(other.Path, StringComparison.Ordinal) && SourceType == other.SourceType;
        public override int GetHashCode() => _hashCode;
        public override bool Equals(object obj) => obj is ComparisonSource other && Equals(other);
        public static bool operator ==(ComparisonSource left, ComparisonSource right) => left.Equals(right);
        public static bool operator !=(ComparisonSource left, ComparisonSource right) => !(left == right);
        #endregion
    }
}
