namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference between the types of two nodes.
/// </summary>
public record NodeClosingDiff : NodeDiff
{
    /// <summary>
    /// Creates a <see cref="NodeTypeDiff"/>.
    /// </summary>
    public NodeClosingDiff(in Comparison comparison) : base(comparison)
    {
    }
}
