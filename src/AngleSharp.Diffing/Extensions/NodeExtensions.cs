using System;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Extensions
{
    public static class NodeExtensions
    {
        public static bool HasAttributes(this INode node)
        {
            return node is IElement element && element.Attributes.Length > 0;
        }

        public static bool TryGetAttr(this INode node, string attributeName, [NotNullWhen(true)]out IAttr? attribute)
        {
            if (node is IElement element && element.HasAttribute(attributeName))
            {
                attribute = element.Attributes[attributeName];
                return true;
            }
            else
            {
                attribute = default;
                return false;
            }
        }

        public static bool TryGetAttrValue(this INode node, string attributeName, out bool result)
        {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            result = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            return node is IElement element && element.TryGetAttrValue(attributeName, out result);
        }

        public static bool TryGetAttrValue(this INode node, string attributeName, [NotNullWhen(true)]out string result)
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            result = default;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            return node is IElement element && element.TryGetAttrValue(attributeName, out result);
        }

        public static bool TryGetAttrValue<T>(this INode node, string attributeName, out T result) where T : System.Enum
        {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            result = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            return node is IElement element && element.TryGetAttrValue(attributeName, out result);
        }

        public static bool TryGetAttrValue<T>(this INode node, string attributeName, Func<string, T> resultFunc, [NotNullWhen(true)] out T result)
        {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            result = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            return node is IElement element && element.TryGetAttrValue(attributeName, resultFunc, out result);
        }

        public static bool IsSameTypeAs(this INode node, INode other)
        {
            return node.NodeType.Equals(other.NodeType) && node.NodeName.Equals(other.NodeName, StringComparison.Ordinal);
        }
    }
}
