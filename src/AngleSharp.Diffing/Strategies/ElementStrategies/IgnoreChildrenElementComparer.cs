namespace AngleSharp.Diffing.Strategies.ElementStrategies;

/// <summary>
/// Represents the ignore children element comparer.
/// </summary>
public static class IgnoreChildrenElementComparer
{
    private const string DIFF_IGNORE_CHILDREN_ATTRIBUTE = "diff:ignorechildren";

    /// <summary>
    /// The ignore children element comparer.
    /// </summary>
    public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
    {
        if (currentDecision.Decision.HasFlag(CompareDecision.SkipChildren) || currentDecision.Decision.HasFlag(CompareDecision.Skip))
            return currentDecision;

        if (!ControlHasTruthyIgnoreChildrenAttribute(comparison))
            return currentDecision;

        return currentDecision.Decision switch
        {
            CompareDecision.None => CompareResult.SkipChildren,
            CompareDecision.Same => CompareResult.SkipChildren,
            CompareDecision.Different => CompareResult.DifferentAndSkipChildren,
            CompareDecision.DifferentAndSkipAttributes => CompareResult.DifferentAndSkipChildrenAndSkipAttributes,
            CompareDecision.SkipAttributes => CompareResult.SkipChildrenAndAttributes,
            _ => currentDecision,
        };
    }

    private static bool ControlHasTruthyIgnoreChildrenAttribute(in Comparison comparison)
    {
        return comparison.Control.Node is IElement element &&
                element.TryGetAttrValue(DIFF_IGNORE_CHILDREN_ATTRIBUTE, out bool shouldIgnore) &&
                shouldIgnore;
    }
}
