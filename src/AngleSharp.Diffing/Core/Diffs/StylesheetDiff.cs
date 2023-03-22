namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference between the applied style of two nodes
/// </summary>
public record StylesheetDiff : NodeDiff
{
    /// <summary>
    /// Creates a <see cref="StylesheetDiff"/>.
    /// </summary>
    public StylesheetDiff(in Comparison comparison) : base(comparison)
    {
    }
}
