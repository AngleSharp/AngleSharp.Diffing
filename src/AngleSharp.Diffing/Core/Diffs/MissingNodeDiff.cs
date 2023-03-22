namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a missing node in the test DOM tree.
/// </summary>
public record MissingNodeDiff : MissingDiffBase<ComparisonSource>
{
    /// <summary>
    /// Creates a <see cref="MissingNodeDiff"></see>.
    /// </summary>
    public MissingNodeDiff(in ComparisonSource control) : base(control, control.Node.NodeType.ToDiffTarget())
    {
    }
}
