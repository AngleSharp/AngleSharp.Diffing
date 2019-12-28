using AngleSharp.Dom;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represent a target of a comparison.
    /// </summary>
    public enum DiffTarget
    {
        /// <summary>
        /// The target is not known.
        /// </summary>
        None,
        /// <summary>
        /// The target is an attribute.
        /// </summary>
        Attribute,
        /// <summary>
        /// The target is a comment.
        /// </summary>
        Comment,
        /// <summary>
        /// The target is an element.
        /// </summary>
        Element,
        /// <summary>
        /// The target is a node.
        /// </summary>
        Node,
        /// <summary>
        /// The target is a text node.
        /// </summary>
        Text
    }

    /// <summary>
    /// Helper methods for working with <see cref="DiffTarget"/>.
    /// </summary>
    public static class NodeTypeExtensions
    {
        /// <summary>
        /// Gets the diff target based on the node type.
        /// </summary>
        /// <param name="nodeType">Mode type to get the diff target off.</param>
        public static DiffTarget ToDiffTarget(this NodeType nodeType)
        {
            return nodeType switch
            {
                NodeType.Element => DiffTarget.Element,
                NodeType.Comment => DiffTarget.Comment,
                NodeType.Text => DiffTarget.Text,
                _ => DiffTarget.Node
            };
        }
    }

}
