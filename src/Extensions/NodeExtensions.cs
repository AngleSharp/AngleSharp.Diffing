using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Extensions
{
    public static class NodeExtensions
    {
        public static bool HasAttributes(this INode node)
        {
            return node is IElement element && element.Attributes.Length > 0;
        }
    }
}
