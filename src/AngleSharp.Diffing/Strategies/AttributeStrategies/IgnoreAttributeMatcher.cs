namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

/// <summary>
/// Ignore Attribute matcher strategy.
/// </summary>
public static class IgnoreAttributeMatcher
{
    private const string DIFF_IGNORE_POSTFIX = ":ignore";

    /// <summary>
    /// Attribute name matcher strategy.
    /// </summary>
    public static IEnumerable<AttributeComparison> Match(IDiffContext context, SourceMap controlSources, SourceMap testSources)
    {
        if (controlSources is null)
            throw new ArgumentNullException(nameof(controlSources));
        if (testSources is null)
            throw new ArgumentNullException(nameof(testSources));

        foreach (var control in controlSources.GetUnmatched())
        {
            // An unmatched :ignore attribute can just be matched with itself if it isn't
            // matched with a "test" attribute of the same name already.
            // this means an ignored attribute is ignored even if it does not appear in the test html.
            if (control.Attribute.Name.EndsWith(DIFF_IGNORE_POSTFIX, StringComparison.OrdinalIgnoreCase))
                yield return new AttributeComparison(control, control);
        }
    }
}