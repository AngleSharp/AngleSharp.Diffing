using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;
using AngleSharp.Diffing.Extensions;
using System.Linq;

namespace AngleSharp.Diffing.Core
{
    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Path should be in lower case")]
    [DebuggerDisplay("{Index} : {Path}")]
    public readonly struct ComparisonSource : IEquatable<ComparisonSource>, IComparisonSource
    {
        private readonly int _hashCode;

        public const char PathSeparatorChar = '>';

        public INode Node { get; }

        public int Index { get; }

        public string Path { get; }

        public ComparisonSourceType SourceType { get; }

        public ComparisonSource(INode node, ComparisonSourceType sourceType)
            : this(node, GetNodeIndex(node), CalculateParentPath(node), sourceType) { }

        public ComparisonSource(INode node, int index, string parentsPath, ComparisonSourceType sourceType)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            var pathSegment = GetNodePathSegment(node);
            Node = node;
            Index = index;
            Path = string.IsNullOrEmpty(parentsPath)
                ? pathSegment
                : CombinePath(parentsPath, pathSegment);

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

        private static string CalculateParentPath(INode node)
        {
            var result = string.Empty;
            foreach (var parent in node.GetParents().TakeWhile(x => x.Parent is { }))
            {
                var pathSegment = GetNodePathSegment(parent);
                if (pathSegment is { })
                    result = CombinePath(pathSegment, result);
            }
            return result;
        }

        private static int GetPathIndex(INode node)
        {
            var result = 0;
            var parent = node.Parent;
            var childNodes = parent.ChildNodes;
            for (int index = 0; index < childNodes.Length; index++)
            {
                if (ReferenceEquals(childNodes[index], node))
                    return result;
                if(childNodes[index] is IParentNode)
                    result += 1;
            }
            throw new InvalidOperationException("Unexpected node tree state. The node was not found in its parents child nodes collection.");
        }

        public static string GetNodePathSegment(INode node)
        {
            var index = GetPathIndex(node);
            return $"{node.NodeName.ToLowerInvariant()}({index})";
        }

        public static string CombinePath(string parentPath, string path)
        {
            if(string.IsNullOrWhiteSpace(parentPath))
                return path;
            if(string.IsNullOrWhiteSpace(path))
                return parentPath;
            return $"{parentPath} {PathSeparatorChar} {path}";
        }

        #region Equals and HashCode
        public bool Equals(ComparisonSource other) => Object.ReferenceEquals(Node, other.Node) && Index == other.Index && Path.Equals(other.Path, StringComparison.Ordinal) && SourceType == other.SourceType;
        public override int GetHashCode() => _hashCode;
        public override bool Equals(object? obj) => obj is ComparisonSource other && Equals(other);
        public static bool operator ==(ComparisonSource left, ComparisonSource right) => left.Equals(right);
        public static bool operator !=(ComparisonSource left, ComparisonSource right) => !left.Equals(right);
        #endregion
    }
}
