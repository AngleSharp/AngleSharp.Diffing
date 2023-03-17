namespace AngleSharp.Diffing.Strategies.CommentStrategies;

/// <summary>
/// Represents the ignore comment filter strategy.
/// </summary>
public static class IgnoreCommentsFilter
{
    /// <summary>
    /// The ignore comment filter strategy.
    /// </summary>
    public static FilterDecision Filter(in ComparisonSource source, FilterDecision currentDecision)
    {
        if (currentDecision.IsExclude())
            return currentDecision;

        if (source.Node.NodeType == NodeType.Comment)
            return FilterDecision.Exclude;

        return currentDecision;
    }
}
