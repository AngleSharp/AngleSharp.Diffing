namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents an unexpected node in the test DOM tree.
/// </summary>
public record class UnexpectedNodeDiff : UnexpectedDiffBase<ComparisonSource>
{
    /// <summary>
    /// Creates a <see cref="UnexpectedNodeDiff"/>.
    /// </summary>
    public UnexpectedNodeDiff(in ComparisonSource test) : base(test, test.Node.NodeType.ToDiffTarget())
    {
    }
}
