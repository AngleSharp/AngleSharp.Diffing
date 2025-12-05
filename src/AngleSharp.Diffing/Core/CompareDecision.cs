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
    /// <summary>
    /// Use when the comparison is different and should skip children.
    /// </summary>
    DifferentAndSkipChildren = Different | SkipChildren,
    /// <summary>
    /// Use when the comparison is different and should skip attributes.
    /// </summary>
    DifferentAndSkipAttributes = Different | SkipAttributes,
    /// <summary>
    /// Use when the comparison is different and should skip both children and attributes.
    /// </summary>
    DifferentAndSkipChildrenAndSkipAttributes = Different | SkipChildren | SkipAttributes,
}

