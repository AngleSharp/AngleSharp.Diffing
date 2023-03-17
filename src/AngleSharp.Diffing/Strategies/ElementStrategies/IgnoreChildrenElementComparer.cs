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
        if (currentDecision == CompareResult.Skip)
            return currentDecision;

        return ControlHasTruthyIgnoreChildrenAttribute(comparison)
            ? CompareResult.SkipChildren
            : currentDecision;
    }

    private static bool ControlHasTruthyIgnoreChildrenAttribute(in Comparison comparison)
    {
        return comparison.Control.Node is IElement element &&
                element.TryGetAttrValue(DIFF_IGNORE_CHILDREN_ATTRIBUTE, out bool shouldIgnore) &&
                shouldIgnore;
    }
}
