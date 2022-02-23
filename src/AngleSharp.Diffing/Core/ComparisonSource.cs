using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AngleSharp.Diffing.Extensions;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a node source in a comparison.
    /// </summary>    
    [DebuggerDisplay("{Index} : {Path}")]
    public readonly struct ComparisonSource : IEquatable<ComparisonSource>, IComparisonSource
    {
        private readonly int _hashCode;

        /// <summary>
        /// Gets the character used as a separator in paths.
        /// </summary>
        public const char PathSeparatorChar = '>';

        /// <summary>
        /// Gets the node represented by the source.
        /// </summary>
        public INode Node { get; }

        /// <summary>
        /// Gets the node's absolute index/position in the DOM tree.
        /// </summary>
        public int Index { get; }

        /// <inheritdoc/>
        public string Path { get; }

        /// <inheritdoc/>
        public ComparisonSourceType SourceType { get; }

        /// <summary>
        /// Creates a <see cref="ComparisonSource"/>.
        /// </summary>
        /// <param name="node">The node of the source.</param>
        /// <param name="sourceType">The source type.</param>
        public ComparisonSource(INode node, ComparisonSourceType sourceType)
            : this(node, GetNodeIndex(node), CalculateParentPath(node), sourceType) { }

        /// <summary>
        /// Creates a <see cref="ComparisonSource"/>.
        /// </summary>
        /// <param name="node">The node of the source.</param>
        /// <param name="index">The node's absolute index/position in the DOM tree</param>
        /// <param name="parentsPath">The path to the parent node in the DOM tree.</param>
        /// <param name="sourceType">The source type.</param>
        public ComparisonSource(INode node, int index, string parentsPath, ComparisonSourceType sourceType)
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));

            var pathSegment = GetNodePathSegment(node);
            Node = node;
            Index = index;
            Path = string.IsNullOrEmpty(parentsPath)
                ? pathSegment
                : CombinePath(parentsPath, pathSegment);

            SourceType = sourceType;
            _hashCode = (Node, Index, Path, SourceType).GetHashCode();
        }

        /// <summary>
        /// Create the last part of the node's path.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Path should be in lower case")]
        public static string GetNodePathSegment(INode node)
        {
            var index = GetPathIndex(node);
            return $"{node.NodeName.ToLowerInvariant()}({index})";
        }

        /// <summary>
        /// Combines a parent path with a path segment.
        /// </summary>
        public static string CombinePath(string parentPath, string path)
        {
            if (string.IsNullOrWhiteSpace(parentPath))
                return path;
            if (string.IsNullOrWhiteSpace(path))
                return parentPath;
            return $"{parentPath} {PathSeparatorChar} {path}";
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
            foreach (var parent in node.GetParents().TakeWhile(x => x.Parent is not null))
            {
                var pathSegment = GetNodePathSegment(parent);
                if (pathSegment is not null)
                    result = CombinePath(pathSegment, result);
            }
            return result;
        }

        private static int GetPathIndex(INode node)
        {
            var parent = node.Parent;
            if (parent is not null)
            {
                var childNodes = parent.ChildNodes;
                for (int index = 0; index < childNodes.Length; index++)
                {
                    if (ReferenceEquals(childNodes[index], node))
                        return index;
                }
            }
            throw new InvalidOperationException("Unexpected node tree state. The node was not found in its parents child nodes collection.");
        }

        #region Equals and HashCode
        /// <inheritdoc/>
        public bool Equals(ComparisonSource other) => Object.ReferenceEquals(Node, other.Node) && Index == other.Index && Path.Equals(other.Path, StringComparison.Ordinal) && SourceType == other.SourceType;
        /// <inheritdoc/>
        public override int GetHashCode() => _hashCode;
        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is ComparisonSource other && Equals(other);
        /// <inheritdoc/>
        public static bool operator ==(ComparisonSource left, ComparisonSource right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(ComparisonSource left, ComparisonSource right) => !left.Equals(right);
        #endregion
    }
}
