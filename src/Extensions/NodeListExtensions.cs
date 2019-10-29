using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing
{
    public static class AngleSharpDomExtensions
    {
        public static SourceCollection ToSourceCollection(this INodeList nodelist, ComparisonSourceType sourceType, string path = "")
        {
            return new SourceCollection(sourceType, nodelist.ToComparisonSourceList(sourceType, path));
        }

        public static IEnumerable<ComparisonSource> ToComparisonSourceList(this INodeList nodes, ComparisonSourceType sourceType, string path = "")
        {
            if (nodes is null) throw new ArgumentNullException(nameof(nodes));

            for (int index = 0; index < nodes.Length; index++)
            {
                yield return nodes[index].ToComparisonSource(index, sourceType, path);
            }
            yield break;
        }

        public static ComparisonSource ToComparisonSource(this INode node, int index, ComparisonSourceType sourceType, string path = "") => new ComparisonSource(node, index, path, sourceType);
    }
}
