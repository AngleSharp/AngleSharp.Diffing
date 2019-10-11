using System.Collections.Generic;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Egil.AngleSharp.Diffing.Core
{
    public abstract class DiffingTestBase
    {
        private readonly IBrowsingContext _context = BrowsingContext.New();
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;

        protected INodeList EmptyNodeList => ToNodeList("");

        protected DiffingTestBase()
        {
            _htmlParser = _context.GetService<IHtmlParser>();
            _document = _context.OpenNewAsync().Result;
        }

        protected INodeList ToNodeList(string? htmlsnippet)
        {
            var fragment = _htmlParser.ParseFragment(htmlsnippet, _document.Body);
            return fragment;
        }

        protected IEnumerable<ComparisonSource> ToComparisonSourceList(string html)
        {
            return ToNodeList(html).ToComparisonSourceList(ComparisonSourceType.Control);
        }

        protected INode ToNode(string htmlsnippet)
        {
            var fragment = _htmlParser.ParseFragment(htmlsnippet, _document.Body);
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
            var element = (IElement)elementSource.Node;
            return new AttributeComparisonSource(element.Attributes[attrName], elementSource);
        }

        protected AttributeComparison ToAttributeComparison(string controlHtml, string controlAttrName, string testHtml, string testAttrName)
        {
            return new AttributeComparison(
                ToAttributeComparisonSource(controlHtml, controlAttrName, ComparisonSourceType.Control),
                ToAttributeComparisonSource(testHtml, testAttrName, ComparisonSourceType.Test)
                );
        }
    }
}