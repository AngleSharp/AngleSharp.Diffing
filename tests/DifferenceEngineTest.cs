using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
    public class DifferenceEngineTest
    {
        private readonly IBrowsingContext _context = BrowsingContext.New();
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;
        private readonly DifferenceEngine sut;

        private INode EmptyNode => CreateTextNode();
        private INodeList EmptyNodeList => ToNodeList("");

        // control node != test node -> comparisons -> differences 
        public DifferenceEngineTest()
        {
            sut = new DifferenceEngine();
            _htmlParser = _context.GetService<IHtmlParser>();
            _document = _context.OpenNewAsync().Result;
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [Fact(DisplayName = "Calling compare with null params throws")]
        public void CompareNullParamsTHrows()
        {
            Should.Throw<ArgumentNullException>(() => sut.Compare(null, null))
                .ParamName.ShouldBe("controlNodes");

            Should.Throw<ArgumentNullException>(() => sut.Compare(EmptyNodeList, null))
                .ParamName.ShouldBe("testNodes");
        }

        [Fact(DisplayName = "Setting NodeFilter to null throws")]
        public void NullNodeFilterThrows()
        {
            Should.Throw<ArgumentNullException>(() => sut.NodeFilter = null)
                .ParamName.ShouldBe("NodeFilter");
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [Fact(DisplayName = "Comparing two empty nodelists results in empty diff")]
        public void EmptyNodeListGivesEmptyDiff()
        {
            var result = sut.Compare(EmptyNodeList, EmptyNodeList);

            result.ShouldBeEmpty();
        }

        // [Theory(DisplayName = "When text-nodes are compared, whitespace is trimmed before and after end of text nodes by default")]
        // [InlineData('\u000C')]
        // [InlineData('\u000A')]
        // [InlineData('\u000D')]
        // [InlineData('\u0009')]
        // [InlineData('\u000B')]
        // [InlineData('\u0085')]
        // public void MyTheory(char whitespace)
        // {
        //     var result = sut.Compare(ToNodeList(control), ToNodeList(test));
        // }

        [Fact(DisplayName = "Compare uses its NodeFilter to select nodes for comparison")]
        public void UsesNodeFilter()
        {
            var nodeFilterCalledTimes = 0;
            sut.NodeFilter = (node) =>
            {
                nodeFilterCalledTimes++;
                return true;
            };
            
            sut.Compare(ToNodeList(@"control"), ToNodeList(@"test"));

            nodeFilterCalledTimes.ShouldBe(2);
        }

        private INodeList ToNodeList(string htmlsnippet)
        {
            var fragment = _htmlParser.ParseFragment(htmlsnippet, _document.Body);
            return fragment;
        }

        private IText CreateTextNode(string? text = null)
        {
            return _document.CreateTextNode(text ?? string.Empty);
        }
    }
}
