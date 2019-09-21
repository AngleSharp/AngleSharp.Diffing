using System.Collections.Generic;
using AngleSharp.Dom;

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
    }
}
