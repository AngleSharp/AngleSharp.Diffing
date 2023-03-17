namespace AngleSharp.Diffing.Core;

internal class DiffContext : IDiffContext
{
    private readonly IElement? _controlRoot;
    private readonly IElement? _testRoot;

    internal HashSet<ComparisonSource> MissingSources { get; } = new HashSet<ComparisonSource>();

    internal HashSet<ComparisonSource> UnexpectedSources { get; } = new HashSet<ComparisonSource>();

    internal HashSet<AttributeComparisonSource> MissingAttributeSources { get; } = new HashSet<AttributeComparisonSource>();

    internal HashSet<AttributeComparisonSource> UnexpectedAttributeSources { get; } = new HashSet<AttributeComparisonSource>();

    public DiffContext(SourceCollection controlSources, SourceCollection testSources)
    {
        if (controlSources.Count > 0 && controlSources[0].Node.GetRoot() is IElement r1)
        { _controlRoot = r1; }
        if (testSources.Count > 0 && testSources[0].Node.GetRoot() is IElement r2)
        { _testRoot = r2; }
    }

    public DiffContext(IElement? controlRoot, IElement? testRoot)
    {
        _controlRoot = controlRoot;
        _testRoot = testRoot;
    }

    public IHtmlCollection<IElement> QueryControlNodes(string selector)
    {
        if (_controlRoot is null)
            return EmptyHtmlCollection<IElement>.Empty;
        return _controlRoot.QuerySelectorAll(selector);
    }

    public IHtmlCollection<IElement> QueryTestNodes(string selector)
    {
        if (_testRoot is null)
            return EmptyHtmlCollection<IElement>.Empty;
        return _testRoot.QuerySelectorAll(selector);
    }

    internal IEnumerable<IDiff> GetDiffsFromUnmatched()
    {
        foreach (var source in MissingSources)
            yield return new MissingNodeDiff(source);
        foreach (var source in UnexpectedSources)
            yield return new UnexpectedNodeDiff(source);
        foreach (var source in MissingAttributeSources)
            yield return new MissingAttrDiff(source);
        foreach (var source in UnexpectedAttributeSources)
            yield return new UnexpectedAttrDiff(source);
    }
}
