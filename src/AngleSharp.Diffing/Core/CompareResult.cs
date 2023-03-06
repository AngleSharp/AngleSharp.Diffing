namespace AngleSharp.Diffing.Core;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public record struct CompareResult(CompareResultDecision Decision, IDiff? Diff = null)
{
    /// <summary>
    /// Use when the compare result is unknown.
    /// </summary>
    public static readonly CompareResult Unknown = default;

    /// <summary>
    /// Use when the two compared nodes or attributes are the same.
    /// </summary>
    public static readonly CompareResult Same = new CompareResult(CompareResultDecision.Same);

    /// <summary>
    /// Use when the two compared nodes or attributes are the different.
    /// </summary>
    public static CompareResult Different(IDiff? diff = null) => new CompareResult(CompareResultDecision.Different, diff);

    /// <summary>
    /// Use when the comparison should be skipped and any child-nodes or attributes skipped as well.
    /// </summary>
    public static readonly CompareResult Skip = new CompareResult(CompareResultDecision.Skip);

    /// <summary>
    /// Use when the comparison should skip any child-nodes.
    /// </summary>
    public static readonly CompareResult SkipChildren = new CompareResult(CompareResultDecision.SkipChildren);

    /// <summary>
    /// Use when the comparison should skip any attributes.
    /// </summary>
    public static readonly CompareResult SkipAttributes = new CompareResult(CompareResultDecision.SkipAttributes);

}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Represents a result of a comparison.
/// </summary>
[Flags]
public enum CompareResultDecision
{
    /// <summary>
    /// Use when the compare result is unknown.
    /// </summary>
    Unknown = 0,
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

/// <summary>
/// Helper methods for <see cref="CompareResult"/>
/// </summary>
public static class CompareResultExtensions
{
    /// <summary>
    /// Checks if a <see cref="CompareResult"/> is either a <see cref="CompareResult.Same"/> or <see cref="CompareResult.Skip"/>.
    /// </summary>
    /// <param name="compareResult">The compare result</param>
    public static bool IsSameOrSkip(this CompareResult compareResult) => compareResult == CompareResult.Same || compareResult == CompareResult.Skip;
}

