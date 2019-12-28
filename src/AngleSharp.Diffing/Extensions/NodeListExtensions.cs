using System;
using System.Collections.Generic;

using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace AngleSharp.Diffing
{
    /// <summary>
    /// Helper methods for working with AngleSharp types.
    /// </summary>
    public static class AngleSharpDomExtensions
    {
        /// <summary>
        /// Creates a source collection from a node list.
        /// </summary>
        public static SourceCollection ToSourceCollection(this IEnumerable<INode> nodelist, ComparisonSourceType sourceType, string path = "")
        {
            return new SourceCollection(sourceType, nodelist.ToComparisonSourceList(sourceType, path));
        }

        /// <summary>
        /// Creates a comparison source list from a node list.
        /// </summary>
        public static IEnumerable<ComparisonSource> ToComparisonSourceList(this IEnumerable<INode> nodes, ComparisonSourceType sourceType, string path = "")
        {
            if (nodes is null)
                throw new ArgumentNullException(nameof(nodes));

            var index = 0;
            foreach (var node in nodes)
            {
                yield return node.ToComparisonSource(index, sourceType, path);
                index += 1;
            }
            yield break;
        }

        /// <summary>
        /// Creates a comparison source from a node.
        /// </summary>
        public static ComparisonSource ToComparisonSource(this INode node, int index, ComparisonSourceType sourceType, string path = "")
            => new ComparisonSource(node, index, path, sourceType);
    }
}
