namespace AngleSharp.Diffing.Core.Diffs;

/// <summary>
/// Defines the reason of two elements to be different.
/// </summary>
public enum ElementDiffKind
{
    /// <summary>
    /// The attribute difference is unspecified.
    /// </summary>
    Unspecified = 0,
    /// <summary>
    /// The type/name of the elements are different.
    /// </summary>
    Name,
    /// <summary>
    /// The two elements have difference tag closing styles.
    /// </summary>
    ClosingStyle,
}
