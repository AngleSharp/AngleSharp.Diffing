using AngleSharp.Html.Parser;

namespace AngleSharp.Diffing;

public class DiffingTestFixture
{
    private readonly IBrowsingContext _context;
    private readonly IHtmlParser _htmlParser;
    private readonly IDocument _document;

    public DiffingTestFixture()
    {
        // Create a custom config with a parser to allow access to the source reference from the AST.
        var config = Configuration.Default
            .WithCss()
            .With<IHtmlParser>(ctx => new HtmlParser(new HtmlParserOptions { IsKeepingSourceReferences = true, IsScripting = ctx?.IsScripting() ?? false }, ctx));

        _context = BrowsingContext.New(config);
        _htmlParser = _context.GetService<IHtmlParser>();
        _document = _context.OpenNewAsync().Result;
    }

    public INodeList Parse(string? html)
    {
        return _htmlParser.ParseFragment(html, _document.Body);
    }
}