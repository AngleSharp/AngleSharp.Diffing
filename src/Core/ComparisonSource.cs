using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Core
{
    [DebuggerDisplay("{Index} : {Path}")]
    public readonly struct ComparisonSource : IEquatable<ComparisonSource>, IComparisonSource
    {
        private readonly int _hashCode;

        public INode Node { get; }

        public int Index { get; }

        public string Path { get; }

        public ComparisonSourceType SourceType { get; }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Path should be in lower case")]
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

        #region Equals and HashCode
        public bool Equals(ComparisonSource other) => Node.Equals(other.Node) && Index == other.Index && Path.Equals(other.Path, StringComparison.Ordinal) && SourceType == other.SourceType;
        public override int GetHashCode() => _hashCode;
        public override bool Equals(object obj) => obj is ComparisonSource other && Equals(other);
        public static bool operator ==(ComparisonSource left, ComparisonSource right) => left.Equals(right);
        public static bool operator !=(ComparisonSource left, ComparisonSource right) => !(left == right);
        #endregion
    }
}
