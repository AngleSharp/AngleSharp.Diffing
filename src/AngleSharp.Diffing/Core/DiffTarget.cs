namespace AngleSharp.Diffing.Core;

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
