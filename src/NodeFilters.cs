using System;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    public static class Filters
    {
        public static bool ElementIgnoreAttributeFilter(INode node)
        {
            if (node is IElement element)
            {
                var ignoreAttr = element.Attributes["diff:ignore"];
                return ignoreAttr == null || !ignoreAttr.IsEmptyOrEquals("TRUE");
            }
            return true;
        }
    }

    public static class AttrExtensions
    {
        public static bool IsEmptyOrEquals(this IAttr attr, string testValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            attr = attr ?? throw new ArgumentNullException(nameof(attr));
            var value = attr.Value;
            return string.IsNullOrWhiteSpace(value) || value.Equals(testValue, comparison);
        }
    }
}
