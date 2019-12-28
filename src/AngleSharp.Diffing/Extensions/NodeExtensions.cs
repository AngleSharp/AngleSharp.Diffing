using System;
using System.Diagnostics.CodeAnalysis;

using AngleSharp.Dom;

namespace AngleSharp.Diffing.Extensions
{
    /// <summary>
    /// Helper methods for working with AngleSharp nodes.
    /// </summary>
    public static class NodeExtensions
    {
        /// <summary>
        /// Gets whether the <paramref name="node"/> has attributes.
        /// </summary>
        public static bool HasAttributes(this INode node)
        {
            return node is IElement element && element.Attributes.Length > 0;
        }

        /// <summary>
        /// Try to get an attribute with the <paramref name="attributeName"/> from the <paramref name="node"/>.
        /// Returns true if the attribute exists, false otherwise.
        /// </summary>
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

        /// <summary>
        /// Try to get an attribute value off of an node.
        /// Returns true when the attribute was found, false otherwise.
        /// </summary>
        public static bool TryGetAttrValue(this INode node, string attributeName, out bool result)
        {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            result = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            return node is IElement element && element.TryGetAttrValue(attributeName, out result);
        }

        /// <summary>
        /// Try to get an attribute value off of an element.
        /// Returns true when the attribute was found, false otherwise.
        /// </summary>
        public static bool TryGetAttrValue(this INode node, string attributeName, [NotNullWhen(true)]out string result)
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            result = default;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            return node is IElement element && element.TryGetAttrValue(attributeName, out result);
        }

        /// <summary>
        /// Try to get an attribute value off of an element.
        /// Returns true when the attribute was found, false otherwise.
        /// </summary>
        public static bool TryGetAttrValue<T>(this INode node, string attributeName, out T result) where T : System.Enum
        {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            result = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            return node is IElement element && element.TryGetAttrValue(attributeName, out result);
        }

        /// <summary>
        /// Try to get an attribute value off of an element.
        /// Returns true when the attribute was found, false otherwise.
        /// </summary>
        public static bool TryGetAttrValue<T>(this INode node, string attributeName, Func<string, T> resultFunc, [NotNullWhen(true)] out T result)
        {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            result = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            return node is IElement element && element.TryGetAttrValue(attributeName, resultFunc, out result);
        }

        /// <summary>
        /// Gets whether two nodes is of the same type (NodeType) and has the same name (NodeName).
        /// </summary>
        public static bool IsSameTypeAs(this INode node, INode other)
        {
            return node.NodeType.Equals(other.NodeType) && node.NodeName.Equals(other.NodeName, StringComparison.Ordinal);
        }
    }
}
