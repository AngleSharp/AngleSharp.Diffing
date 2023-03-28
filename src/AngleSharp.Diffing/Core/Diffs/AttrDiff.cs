namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents an attribute difference.
/// </summary>
public record class AttrDiff : DiffBase<AttributeComparisonSource>
{
    /// <summary>
    /// Gets the kind of the diff.
    /// </summary>
    public AttrDiffKind Kind { get; }

    /// <summary>
    /// Creates an <see cref="AttrDiff"/>.
    /// </summary>
    public AttrDiff(in AttributeComparison comparison, AttrDiffKind kind) : base(comparison.Control, comparison.Test, DiffTarget.Attribute)
    {
        Kind = kind;
    }
}
