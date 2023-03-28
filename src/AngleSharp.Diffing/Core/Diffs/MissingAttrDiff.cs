namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a missing attribute in a test node.
/// </summary>
public record class MissingAttrDiff : MissingDiffBase<AttributeComparisonSource>
{
    /// <summary>
    /// Creates a <see cref="MissingAttrDiff"/>.
    /// </summary>
    public MissingAttrDiff(in AttributeComparisonSource control) : base(control, DiffTarget.Attribute)
    {
    }
}
