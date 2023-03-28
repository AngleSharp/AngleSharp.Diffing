namespace AngleSharp.Diffing.Strategies.CommentStrategies;

/// <summary>
/// Represents the comment comparer strategy.
/// </summary>
public static class CommentComparer
{
    /// <summary>
    /// The comment comparer strategy.
    /// </summary>
    public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
    {
        if (currentDecision.IsSameOrSkip)
            return currentDecision;

        if (comparison.TryGetNodesAsType<IComment>(out var controlComment, out var testComment))
            return controlComment.Data.Equals(testComment.Data, StringComparison.Ordinal)
                ? CompareResult.Same
                : CompareResult.FromDiff(new CommentDiff(comparison));
        else
            return currentDecision;
    }
}
