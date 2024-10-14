namespace AngleSharp.Diffing;

public abstract class DiffingTestBase : IClassFixture<DiffingTestFixture>
{
    private readonly DiffingTestFixture _testFixture;


    public static readonly TheoryData<CompareResult> SameAndSkipCompareResult = new()
    {
        CompareResult.Same,
        CompareResult.Skip,
    };

    protected IDiffContext DummyContext { get; } = new DiffContext(default(IElement), default(IElement));

    protected INodeList EmptyNodeList => ToNodeList("");

    protected DiffingTestBase(DiffingTestFixture fixture)
    {
        _testFixture = fixture;
    }

    protected INodeList ToNodeList(string? htmlsnippet)
    {
        var fragment = _testFixture.Parse(htmlsnippet);
        return fragment;
    }

    protected IEnumerable<ComparisonSource> ToComparisonSourceList(string html)
    {
        return ToNodeList(html).ToComparisonSourceList(ComparisonSourceType.Control);
    }

    protected INode ToNode(string htmlsnippet)
    {
        var fragment = _testFixture.Parse(htmlsnippet);
        return fragment[0];
    }

    protected ComparisonSource ToComparisonSource(string html, ComparisonSourceType sourceType = ComparisonSourceType.Control)
    {
        return ToNode(html).ToComparisonSource(0, sourceType);
    }

    protected Comparison ToComparison(string controlHtml, string testHtml)
    {
        return new Comparison(
            ToComparisonSource(controlHtml, ComparisonSourceType.Control),
            ToComparisonSource(testHtml, ComparisonSourceType.Test)
            );
    }

    protected AttributeComparisonSource ToAttributeComparisonSource(string html, string attrName, ComparisonSourceType sourceType = ComparisonSourceType.Control)
    {
        var elementSource = ToComparisonSource(html, sourceType);
        return new AttributeComparisonSource(attrName, elementSource);
    }

    protected AttributeComparison ToAttributeComparison(string controlHtml, string controlAttrName, string testHtml, string testAttrName)
    {
        return new AttributeComparison(
            ToAttributeComparisonSource(controlHtml, controlAttrName, ComparisonSourceType.Control),
            ToAttributeComparisonSource(testHtml, testAttrName, ComparisonSourceType.Test)
            );
    }

    protected SourceCollection ToSourceCollection(string html, ComparisonSourceType sourceType = ComparisonSourceType.Control)
    {
        var sources = ToComparisonSourceList(html);
        return new SourceCollection(sourceType, sources);
    }

    protected SourceMap ToSourceMap(string html, ComparisonSourceType sourceType = ComparisonSourceType.Control)
    {
        var source = ToComparisonSource(html, sourceType);
        return new SourceMap(source);
    }
}