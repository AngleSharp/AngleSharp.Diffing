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
        if (currentDecision.IsSameOrSkip())
            return currentDecision;
        return comparison.Control.Node.NodeType == NodeType.Comment && comparison.AreNodeTypesEqual
            ? CompareResult.Same
            : CompareResult.Different;
    }
}
