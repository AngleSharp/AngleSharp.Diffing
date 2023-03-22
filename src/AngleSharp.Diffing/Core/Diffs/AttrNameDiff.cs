namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents an attribute difference with different names
/// </summary>
public record AttrNameDiff : AttrDiff
{
    /// <summary>
    /// Creates an <see cref="AttrNameDiff"/>.
    /// </summary>
    public AttrNameDiff(in AttributeComparison comparison) : base(comparison)
    {
    }
}
