namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference between two nodes.
/// </summary>
public record NodeDiff : DiffBase<ComparisonSource>
{
    /// <summary>
    /// Creates a <see cref="NodeDiff"/>.
    /// </summary>
    public NodeDiff(in Comparison comparison) : base(comparison.Control, comparison.Test, comparison.Control.Node.NodeType.ToDiffTarget())
    {
    }
}