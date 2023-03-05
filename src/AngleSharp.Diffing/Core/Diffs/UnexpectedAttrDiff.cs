namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents an unexpected attribute found in the test DOM tree.
/// </summary>
[DebuggerDisplay("Unexpected Attribute: Test = {Test.Path}")]
public class UnexpectedAttrDiff : UnexpectedDiffBase<AttributeComparisonSource>
{
    /// <summary>
    /// Creates a <see cref="UnexpectedAttrDiff"/>.
    /// </summary>
    public UnexpectedAttrDiff(in AttributeComparisonSource test) : base(test, DiffTarget.Attribute)
    {
    }
}
