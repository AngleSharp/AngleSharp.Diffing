namespace AngleSharp.Diffing.Core;

/// <summary>
/// Defines the reason of the diff.
/// </summary>
public enum AttrDiffKind
{
    /// <summary>
    /// The attribute difference is unspecified.
    /// </summary>
    Unspecified,
    /// <summary>
    /// The name of the attribute is different.
    /// </summary>
    Name,
    /// <summary>
    /// The value of the attribute is different.
    /// </summary>
    Value,
}
