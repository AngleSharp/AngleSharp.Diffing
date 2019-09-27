using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    internal static class NodeListExtensions
    {
        internal static IEnumerable<IComparisonSource<INode>> ToComparisonSourceList(this INodeList nodes, ComparisonSourceType sourceType, string path = "")
        {
            for (int index = 0; index < nodes.Length; index++)
            {
                var node = nodes[index];

                switch (node)
                {
                    case IElement elm:
                        yield return new ComparisonSource<IElement>(elm, index, path, sourceType);
                        break;
                    case IComment comment:
                        yield return new ComparisonSource<IComment>(comment, index, path, sourceType);
                        break;
                    case IText text:
                        yield return new ComparisonSource<IText>(text, index, path, sourceType);
                        break;
                    default:
                        yield return new ComparisonSource<INode>(node, index, path, sourceType);
                        break;
                }
            }
            yield break;
        }
    }
}
