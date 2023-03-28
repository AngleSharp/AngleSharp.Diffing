namespace AngleSharp.Diffing.Core;

/// <summary>
/// Defines the reason for two attributes to be the different.
/// </summary>
public enum AttrDiffKind
{
    /// <summary>
    /// The attribute difference is unspecified.
    /// </summary>
    Unspecified = 0,
    /// <summary>
    /// The name of the attribute is different.
    /// </summary>
    Name,
    /// <summary>
    /// The value of the attribute is different.
    /// </summary>
    Value,
}
