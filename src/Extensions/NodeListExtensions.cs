using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    public static class AngleSharpDomExtensions
    {
        public static IEnumerable<IComparisonSource<INode>> ToComparisonSourceList(this INodeList nodes, ComparisonSourceType sourceType, string path = "")
        {
            if (nodes is null) throw new ArgumentNullException(nameof(nodes));

            for (int index = 0; index < nodes.Length; index++)
            {
                yield return nodes[index].ToComparisonSource(index, sourceType, path);
            }
            yield break;
        }

        public static IComparisonSource<INode> ToComparisonSource(this INode node, int index, ComparisonSourceType sourceType, string path = "")
        {
            switch (node)
            {
                case IElement elm: return new ComparisonSource<IElement>(elm, index, path, sourceType);
                case IComment comment: return new ComparisonSource<IComment>(comment, index, path, sourceType);
                case IText text: return new ComparisonSource<IText>(text, index, path, sourceType);
                default: return new ComparisonSource<INode>(node, index, path, sourceType);
            }
        }

        public static bool IsEmptyOrEquals(this IAttr attr, string testValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            attr = attr ?? throw new ArgumentNullException(nameof(attr));
            var value = attr.Value;
            return string.IsNullOrWhiteSpace(value) || value.Equals(testValue, comparison);
        }

    }
}
