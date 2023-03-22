namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference between two texts.
/// </summary>
public record TextDiff : NodeDiff
{
    /// <summary>
    /// Creates a <see cref="NodeDiff"/>.
    /// </summary>
    public TextDiff(in Comparison comparison) : base(comparison)
    {
    }
}
