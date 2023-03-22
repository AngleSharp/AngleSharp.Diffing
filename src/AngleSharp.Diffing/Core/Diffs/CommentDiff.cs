namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference between two nodes.
/// </summary>
public record CommentDiff : NodeDiff
{
    /// <summary>
    /// Creates a <see cref="NodeDiff"/>.
    /// </summary>
    public CommentDiff(in Comparison comparison) : base(comparison)
    {
    }
}
