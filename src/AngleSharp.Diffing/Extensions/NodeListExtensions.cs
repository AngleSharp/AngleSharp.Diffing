using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Diffing.Core;

namespace AngleSharp.Diffing
{
    public static class AngleSharpDomExtensions
    {
        public static SourceCollection ToSourceCollection(this IEnumerable<INode> nodelist, ComparisonSourceType sourceType, string path = "")
        {
            return new SourceCollection(sourceType, nodelist.ToComparisonSourceList(sourceType, path));
        }

        public static IEnumerable<ComparisonSource> ToComparisonSourceList(this IEnumerable<INode> nodes, ComparisonSourceType sourceType, string path = "")
        {
            if (nodes is null) throw new ArgumentNullException(nameof(nodes));

            var index = 0;
            foreach (var node in nodes)
            {
                yield return node.ToComparisonSource(index, sourceType, path);
                index += 1;
            }
            yield break;
        }

        public static ComparisonSource ToComparisonSource(this INode node, int index, ComparisonSourceType sourceType, string path = "")
            => new ComparisonSource(node, index, path, sourceType);
    }
}
