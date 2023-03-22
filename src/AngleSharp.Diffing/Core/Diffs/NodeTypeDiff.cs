namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference between the types of two nodes.
/// </summary>
public record NodeTypeDiff : NodeDiff
{
    /// <summary>
    /// Creates a <see cref="NodeTypeDiff"/>.
    /// </summary>
    public NodeTypeDiff(in Comparison comparison) : base(comparison)
    {
    }
}
