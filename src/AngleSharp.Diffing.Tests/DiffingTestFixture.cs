using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace AngleSharp.Diffing
{
    public class DiffingTestFixture
    {
        private readonly IBrowsingContext _context;
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;

        public DiffingTestFixture()
        {
            var config = Configuration.Default.WithCss();
            _context = BrowsingContext.New(config);
            _htmlParser = _context.GetService<IHtmlParser>();
            _document = _context.OpenNewAsync().Result;
        }

        public INodeList Parse(string? html)
        {
            return _htmlParser.ParseFragment(html, _document.Body);
        }
    }
}