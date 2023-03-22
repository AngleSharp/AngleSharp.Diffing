namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents an unexpected attribute found in the test DOM tree.
/// </summary>
public record UnexpectedAttrDiff : UnexpectedDiffBase<AttributeComparisonSource>
{
    /// <summary>
    /// Creates a <see cref="UnexpectedAttrDiff"/>.
    /// </summary>
    public UnexpectedAttrDiff(in AttributeComparisonSource test) : base(test, DiffTarget.Attribute)
    {
    }
}
