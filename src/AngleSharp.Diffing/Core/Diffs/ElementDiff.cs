namespace AngleSharp.Diffing.Core.Diffs;

/// <summary>
/// Represents an element difference.
/// </summary>
public record class ElementDiff : NodeDiff
{
    /// <summary>
    /// Gets the kind of the diff.
    /// </summary>
    public ElementDiffKind Kind { get; }

    /// <summary>
    /// Creates an <see cref="ElementDiff"/>.
    /// </summary>
    public ElementDiff(in Comparison comparison, ElementDiffKind kind) : base(comparison)
    {
        Kind = kind;
    }

}
