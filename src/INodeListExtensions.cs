using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    internal static class INodeListExtensions
    {
        internal static IEnumerable<INode> WalkNodeTree(this INodeList nodes)
        {
            foreach (var root in nodes)
            {
                foreach (var node in root.GetDescendantsAndSelf())
                {
                    yield return node;
                }
            }
        }

        internal static IEnumerable<IComparisonSource<INode>> ToComparisonSourceList(this INodeList nodes, string path, ComparisonSourceType sourceType)
        {
            //var result = new List<IComparisonSource<INode>>(nodes.Length);
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
            //return result;
        }
    }
}
