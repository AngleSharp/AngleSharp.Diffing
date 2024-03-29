namespace AngleSharp.Diffing.Strategies.NodeStrategies;

/// <summary>
/// Represents the forward searching node matcher strategy.
/// </summary>
public static class ForwardSearchingNodeMatcher
{
    /// <summary>
    /// The forward searching node matcher strategy.
    /// </summary>
    public static IEnumerable<Comparison> Match(IDiffContext context,
                                         SourceCollection controlSources,
                                         SourceCollection testSources)
    {
        if (controlSources is null)
            throw new ArgumentNullException(nameof(controlSources));
        if (testSources is null)
            throw new ArgumentNullException(nameof(testSources));

        var lastMatchedTestNodeIndex = -1;
        foreach (var control in controlSources.GetUnmatched())
        {
            var comparison = TryFindMatchingNodes(control, testSources, lastMatchedTestNodeIndex + 1);
            if (comparison.HasValue)
            {
                yield return comparison.Value;
                lastMatchedTestNodeIndex = comparison.Value.Test.Index;
            }
        }

        yield break;
    }

    private static Comparison? TryFindMatchingNodes(in ComparisonSource control, SourceCollection testSources, int startIndex)
    {
        foreach (var test in testSources.GetUnmatched(startIndex))
        {
            if (control.Node.IsSameTypeAs(test.Node))
            {
                return new Comparison(control, test);
            }
        }
        return null;
    }
}
