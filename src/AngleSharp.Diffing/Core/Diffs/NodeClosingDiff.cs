namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference between the types of two nodes.
/// </summary>
public class NodeClosingDiff : NodeDiff
{
    /// <summary>
    /// Creates a <see cref="NodeTypeDiff"/>.
    /// </summary>
    public NodeClosingDiff(in Comparison comparison) : base(comparison)
    {
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Node closing diff: Control = {Control.Path}, Test = {Test.Path}";
    }
}
