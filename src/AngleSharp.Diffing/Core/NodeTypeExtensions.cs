namespace AngleSharp.Diffing.Core;

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
