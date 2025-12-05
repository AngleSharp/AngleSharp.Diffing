namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a result of a comparison.
/// </summary>
/// <param name="Decision">Gets the latest <see cref="CompareDecision"/> of the comparison.</param>
/// <param name="Diff">Gets the optional <see cref="IDiff"/> related to the current <paramref name="Decision"/>.</param>
public readonly record struct CompareResult(CompareDecision Decision, IDiff? Diff = null)
{
    /// <summary>
    /// Use when the compare result is unknown.
    /// </summary>
    public static readonly CompareResult Unknown;

    /// <summary>
    /// Use when the two compared nodes or attributes are the same.
    /// </summary>
    public static readonly CompareResult Same = new(CompareDecision.Same);

    /// <summary>
    /// Use when the comparison should be skipped and any child-nodes or attributes skipped as well.
    /// </summary>
    public static readonly CompareResult Skip = new(CompareDecision.Skip);

    /// <summary>
    /// Use when the comparison should skip any child-nodes.
    /// </summary>
    public static readonly CompareResult SkipChildren = new(CompareDecision.SkipChildren);

    /// <summary>
    /// Use when the comparison should skip any attributes.
    /// </summary>
    public static readonly CompareResult SkipAttributes = new(CompareDecision.SkipAttributes);

    /// <summary>
    /// Use when the comparison should skip any attributes but has element differences.
    /// </summary>
    public static readonly CompareResult DifferentAndSkipAttributes = new(CompareDecision.Different | CompareDecision.SkipAttributes);

    /// <summary>
    /// Use when the comparison should skip any attributes and children but has element differences.
    /// </summary>
    public static readonly CompareResult DifferentAndSkipChildrenAndSkipAttributes = new(CompareDecision.Different | CompareDecision.SkipChildren | CompareDecision.SkipAttributes);

    /// <summary>
    /// Use when the comparison should skip children but has element differences.
    /// </summary>
    public static readonly CompareResult DifferentAndSkipChildren = new(CompareDecision.Different | CompareDecision.SkipChildren);

    /// <summary>
    /// Use when the comparison should skip any children and any attributes.
    /// </summary>
    public static readonly CompareResult SkipChildrenAndAttributes = new(CompareDecision.SkipChildren | CompareDecision.SkipAttributes);

    /// <summary>
    /// Use when the two compared nodes or attributes are the different.
    /// </summary>
    public static CompareResult Different => new(CompareDecision.Different);

    /// <summary>
    /// Use when the two compared nodes or attributes are the different.
    /// </summary>
    /// <param name="diff">The associated <see cref="IDiff"/> describing the difference.</param>
    /// <returns>Returns a <see cref="CompareResult"/> with <see cref="CompareResult.Decision"/> set to <see cref="CompareDecision.Different"/>.</returns>
    public static CompareResult FromDiff(IDiff diff) => new(CompareDecision.Different, diff);

    /// <summary>
    /// Checks if a <see cref="CompareResult"/> is either a <see cref="CompareDecision.Same"/> or <see cref="CompareDecision.Skip"/>.
    /// </summary>
    public bool IsSameOrSkip => Decision.HasFlag(CompareDecision.Same) || Decision.HasFlag(CompareDecision.Skip);
}