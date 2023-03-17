namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents the engine that drives the diffing/comparison of two DOM trees.
/// </summary>
public class HtmlDifferenceEngine
{
    private readonly IDiffingStrategy _diffingStrategy;
    private readonly SourceCollection _controlSources;
    private readonly SourceCollection _testSources;

    private DiffContext Context { get; }

    /// <summary>
    /// Creates a diffing engine which will perform a comparison of the control and test sources, using the provided strategies.
    /// </summary>
    public HtmlDifferenceEngine(IDiffingStrategy diffingStrategy, SourceCollection controlSources, SourceCollection testSources)
    {
        _diffingStrategy = diffingStrategy ?? throw new ArgumentNullException(nameof(diffingStrategy));
        _controlSources = controlSources ?? throw new ArgumentNullException(nameof(controlSources));
        _testSources = testSources ?? throw new ArgumentNullException(nameof(testSources));
        Context = new DiffContext(controlSources, testSources);
    }

    /// <summary>
    /// Executes the comparison and returns any differences found.
    /// </summary>
    public IEnumerable<IDiff> Compare()
    {
        var diffs = Compare(_controlSources, _testSources);
        var unmatchedDiffs = Context.GetDiffsFromUnmatched();

        return diffs.Concat(unmatchedDiffs);
    }

    private IEnumerable<IDiff> Compare(SourceCollection controlSources, SourceCollection testSources)
    {
        ApplyNodeFilter(controlSources);
        ApplyNodeFilter(testSources);
        var comparisons = MatchNodes(controlSources, testSources);
        var diffs = CompareNodes(comparisons);

        return diffs;
    }

    private void ApplyNodeFilter(SourceCollection sources) => sources.Remove(_diffingStrategy.Filter);

    private IEnumerable<Comparison> MatchNodes(SourceCollection controls, SourceCollection tests)
    {
        foreach (var comparison in _diffingStrategy.Match(Context, controls, tests))
        {
            UpdateMatchedTracking(comparison);
            yield return comparison;
        }

        UpdateUnmatchedTracking();

        yield break;

        void UpdateMatchedTracking(in Comparison comparison)
        {
            controls.MarkAsMatched(comparison.Control);
            tests.MarkAsMatched(comparison.Test);
            Context.MissingSources.Remove(comparison.Control);
            Context.UnexpectedSources.Remove(comparison.Test);
        }

        void UpdateUnmatchedTracking()
        {
            Context.MissingSources.AddRange(controls.GetUnmatched());
            Context.UnexpectedSources.AddRange(tests.GetUnmatched());
        }
    }

    private IEnumerable<IDiff> CompareNodes(IEnumerable<Comparison> comparisons)
    {
        return comparisons.SelectMany(comparison => CompareNode(comparison));
    }

    private IEnumerable<IDiff> CompareNode(in Comparison comparison)
    {
        if (comparison.Control.Node is IElement)
        {
            return CompareElement(comparison);
        }

        var compareRes = _diffingStrategy.Compare(comparison);
        if (compareRes.HasFlag(CompareResult.Different))
        {
            IDiff diff = new NodeDiff(comparison);
            return new[] { diff };
        }

        return Array.Empty<IDiff>();
    }

    private IEnumerable<IDiff> CompareElement(in Comparison comparison)
    {
        var result = new List<IDiff>();

        var compareRes = _diffingStrategy.Compare(comparison);
        if (compareRes.HasFlag(CompareResult.Different))
        {
            result.Add(new NodeDiff(comparison));
        }

        if (!compareRes.HasFlag(CompareResult.Skip))
        {
            if (!compareRes.HasFlag(CompareResult.SkipAttributes))
                result.AddRange(CompareElementAttributes(comparison));
            if (!compareRes.HasFlag(CompareResult.SkipChildren))
                result.AddRange(CompareChildNodes(comparison));
        }

        return result;
    }

    private IEnumerable<IDiff> CompareElementAttributes(in Comparison comparison)
    {
        if (!comparison.Control.Node.HasAttributes() && !comparison.Test.Node.HasAttributes())
            return Array.Empty<IDiff>();

        var controlAttrs = new SourceMap(comparison.Control);
        var testAttrs = new SourceMap(comparison.Test);

        ApplyFilterAttributes(controlAttrs);
        ApplyFilterAttributes(testAttrs);

        var attrComparisons = MatchAttributes(controlAttrs, testAttrs);

        return CompareAttributes(attrComparisons);
    }

    private void ApplyFilterAttributes(SourceMap controlAttrs)
    {
        controlAttrs.Remove(_diffingStrategy.Filter);
    }

    private IEnumerable<AttributeComparison> MatchAttributes(SourceMap controls, SourceMap tests)
    {
        foreach (var comparison in _diffingStrategy.Match(Context, controls, tests))
        {
            MarkSelectedSourcesAsMatched(comparison);
            yield return comparison;
        }

        UpdateUnmatchedTracking();

        yield break;

        void MarkSelectedSourcesAsMatched(in AttributeComparison comparison)
        {
            controls.MarkAsMatched(comparison.Control);
            tests.MarkAsMatched(comparison.Test);
            Context.MissingAttributeSources.Remove(comparison.Control);
            Context.UnexpectedAttributeSources.Remove(comparison.Test);
        }

        void UpdateUnmatchedTracking()
        {
            Context.MissingAttributeSources.AddRange(controls.GetUnmatched());
            Context.UnexpectedAttributeSources.AddRange(tests.GetUnmatched());
        }
    }

    private IEnumerable<IDiff> CompareChildNodes(in Comparison comparison)
    {
        if (!comparison.Control.Node.HasChildNodes && !comparison.Test.Node.HasChildNodes)
            return Array.Empty<IDiff>();

        var ctrlChildNodes = comparison.Control.Node.ChildNodes;
        var testChildNodes = comparison.Test.Node.ChildNodes;
        var ctrlPath = comparison.Control.Path;
        var testPath = comparison.Test.Path;

        return Compare(
            ctrlChildNodes.ToSourceCollection(ComparisonSourceType.Control, ctrlPath),
            testChildNodes.ToSourceCollection(ComparisonSourceType.Test, testPath)
        );
    }

    private IEnumerable<IDiff> CompareAttributes(IEnumerable<AttributeComparison> comparisons)
    {
        foreach (var comparison in comparisons)
        {
            var compareRes = _diffingStrategy.Compare(comparison);
            if (compareRes == CompareResult.Different)
                yield return new AttrDiff(comparison);
        }
    }
}
