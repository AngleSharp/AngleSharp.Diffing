namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents the decision of a comparison.
/// </summary>
[Flags]
public enum CompareDecision
{
    /// <summary>
    /// Use when the compare result is unknown.
    /// </summary>
    None = 0,
    /// <summary>
    /// Use when the two compared nodes or attributes are the same.
    /// </summary>
    Same = 1,
    /// <summary>
    /// Use when the two compared nodes or attributes are the different.
    /// </summary>
    Different = 2,
    /// <summary>
    /// Use when the comparison should be skipped and any child-nodes or attributes skipped as well.
    /// </summary>
    Skip = 4,
    /// <summary>
    /// Use when the comparison should skip any child-nodes.
    /// </summary>
    SkipChildren = 8,
    /// <summary>
    /// Use when the comparison should skip any attributes.
    /// </summary>
    SkipAttributes = 16,
}

