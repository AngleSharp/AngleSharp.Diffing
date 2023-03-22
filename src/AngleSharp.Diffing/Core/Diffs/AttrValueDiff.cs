namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents an attribute difference with different values
/// </summary>
public record AttrValueDiff : AttrDiff
{
    /// <summary>
    /// The kind of the diff.
    /// </summary>
    public AttrValueDiffKind Kind { get; }

    /// <summary>
    /// Creates an <see cref="AttrValueDiff"/>.
    /// </summary>
    public AttrValueDiff(in AttributeComparison comparison, AttrValueDiffKind kind) : base(comparison)
    {
        Kind = kind;
    }
}

/// <summary>
/// Defines the reason of the diff.
/// </summary>
public enum AttrValueDiffKind
{
    /// <summary>
    /// The values are different.
    /// </summary>
    value,
    /// <summary>
    /// The boolean values are different.
    /// </summary>
    BooleanValue,
    /// <summary>
    /// The classes have different length.
    /// </summary>
    ClassCount,
    /// <summary>
    /// The value is a class.
    /// </summary>
    Classes,
    /// <summary>
    /// The styles are different.
    /// </summary>
    Styles
}
