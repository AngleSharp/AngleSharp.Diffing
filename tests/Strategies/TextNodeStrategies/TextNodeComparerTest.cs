using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeComparerTest : TextNodeTestBase
    {
        public TextNodeComparerTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "When input node is not a IText node, comparer does not run nor change the current decision")]
        public void Test2()
        {
            var comparison = ToComparison("<p></p>", "<p></p>");
            var sut = new TextNodeComparer();

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            sut.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            sut.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        }

        [Theory(DisplayName = "When option is Preserve or RemoveWhitespaceNodes, comparer does not run nor change the current decision")]
        [InlineData(WhitespaceOption.Preserve)]
        [InlineData(WhitespaceOption.RemoveWhitespaceNodes)]
        public void Test5(WhitespaceOption whitespaceOption)
        {
            var comparison = ToComparison("hello world", "  hello   world  ");
            var sut = new TextNodeComparer(whitespaceOption);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            sut.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            sut.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        }

        [Fact(DisplayName = "When option is Normalize and current decision is Same or Skip, compare uses the current decision")]
        public void Test55()
        {
            var comparison = new Comparison();
            var sut = new TextNodeComparer(WhitespaceOption.Normalize);

            sut.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            sut.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        }

        [Theory(DisplayName = "When option is Normalize, any whitespace before and after a text node is removed before comparison")]
        [MemberData(nameof(WhitespaceCharStrings))]
        public void Test7(string whitespace)
        {
            var sut = new TextNodeComparer(WhitespaceOption.Normalize);
            var normalText = "text";
            var whitespaceText = $"{whitespace}text{whitespace}";
            var c1 = ToComparison(normalText    , normalText);
            var c2 = ToComparison(normalText    , whitespaceText);
            var c3 = ToComparison(whitespaceText, normalText);
            var c4 = ToComparison(whitespaceText, whitespaceText);

            sut.Compare(c1, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c2, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c3, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c4, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When option is Normalize, any consecutive whitespace characters are collapsed into one before comparison")]
        [MemberData(nameof(WhitespaceCharStrings))]
        public void Test9(string whitespace)
        {
            var sut = new TextNodeComparer(WhitespaceOption.Normalize);
            var normalText = "hello world";
            var whitespaceText = $"{whitespace}hello{whitespace}{whitespace}world{whitespace}";
            var c1 = ToComparison(normalText, normalText);
            var c2 = ToComparison(normalText, whitespaceText);
            var c3 = ToComparison(whitespaceText, normalText);
            var c4 = ToComparison(whitespaceText, whitespaceText);

            sut.Compare(c1, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c2, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c3, CompareResult.Different).ShouldBe(CompareResult.Same);
            sut.Compare(c4, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When a parent node has a inline whitespace option, that overrides the global whitespace option")]
        [InlineData(@"<header><h1><em diff:whitespace=""normalize""> foo   bar </em></h1></header>", @"<header><h1><em>foo bar</em></h1></header>")]
        [InlineData(@"<header><h1 diff:whitespace=""normalize""><em> foo   bar </em></h1></header>", @"<header><h1><em>foo bar</em></h1></header>")]
        [InlineData(@"<header diff:whitespace=""normalize""><h1><em> foo   bar </em></h1></header>", @"<header><h1><em>foo bar</em></h1></header>")]
        public void Test001(string controlHtml, string testHtml)
        {
            var sut = new TextNodeComparer(WhitespaceOption.Preserve);
            var controlSource = new ComparisonSource(ToNode(controlHtml).FirstChild.FirstChild.FirstChild, 0, "dummypath", ComparisonSourceType.Control);
            var testSource = new ComparisonSource(ToNode(testHtml).FirstChild.FirstChild.FirstChild, 0, "dummypath", ComparisonSourceType.Test);
            var comparison = new Comparison(controlSource, testSource);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When whitespace option is Preserve or RemoveWhitespaceNodes, a string ordinal comparison is performed")]
        [InlineData(WhitespaceOption.Preserve)]
        [InlineData(WhitespaceOption.RemoveWhitespaceNodes)]
        public void Test003(WhitespaceOption whitespaceOption)
        {
            var sut = new TextNodeComparer(whitespaceOption);
            var comparison = ToComparison("  hello\n\nworld ", "  hello\n\nworld ");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When IgnoreCase is true, a string ordinal ignore case comparison is performed")]
        public void Test004()
        {
            var sut = new TextNodeComparer(ignoreCase: true);
            var comparison = ToComparison("HELLO WoRlD", "hello world");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When the parent element is <pre/script/style>, the is implicitly set to Preserve")]
        [InlineData("pre")]
        [InlineData("script")]
        [InlineData("style")]
        public void Test005(string tag)
        {
            var sut = new TextNodeComparer(WhitespaceOption.Normalize);
            var elm = ToComparisonSource($"<{tag}>foo   bar</{tag}>");
            var controlSource = new ComparisonSource(elm.Node.FirstChild, 0, elm.Path, ComparisonSourceType.Control);
            var testSource = ToComparisonSource("foo bar", ComparisonSourceType.Test);
            var comparison = new Comparison(controlSource, testSource);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        }

        [Theory(DisplayName = "When the parent element is <pre/script/style> and the whitespace option is set inline, the inline option is used instead of Preserve")]
        [InlineData("pre")]
        [InlineData("script")]
        [InlineData("style")]
        public void Test006(string tag)
        {
            var sut = new TextNodeComparer(WhitespaceOption.Normalize);
            var elm = ToComparisonSource($"<{tag} diff:whitespace=\"normalize\">foo   bar</{tag}>");
            var controlSource = new ComparisonSource(elm.Node.FirstChild, 0, elm.Path, ComparisonSourceType.Control);
            var testSource = ToComparisonSource("foo bar", ComparisonSourceType.Test);
            var comparison = new Comparison(controlSource, testSource);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When IgnoreCase='true' inline attribute is present in a parent element, a string ordinal ignore case comparison is performed")]
        [InlineData(@"<header><h1><em diff:ignoreCase=""true"">HELLO WoRlD</em></h1></header>")]
        [InlineData(@"<header><h1  diff:ignoreCase=""True""><em>HELLO WoRlD</em></h1></header>")]
        [InlineData(@"<header diff:ignoreCase=""TRUE""><h1><em>HELLO WoRlD</em></h1></header>")]
        public void Test008(string controlHtml)
        {
            var sut = new TextNodeComparer(ignoreCase: false);
            var rootSource = ToComparisonSource(controlHtml);
            var controlSource = new ComparisonSource(rootSource.Node.FirstChild.FirstChild.FirstChild, 0, rootSource.Path, ComparisonSourceType.Control);
            var testSource = ToComparisonSource("hello world", ComparisonSourceType.Test);
            var comparison = new Comparison(controlSource, testSource);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When IgnoreCase='false' inline attribute is present in a parent element, a string ordinal case comparison is performed")]
        [InlineData(@"<header><h1><em diff:ignoreCase=""false"">HELLO WoRlD</em></h1></header>")]
        [InlineData(@"<header><h1  diff:ignoreCase=""False""><em>HELLO WoRlD</em></h1></header>")]
        [InlineData(@"<header diff:ignoreCase=""FALSE""><h1><em>HELLO WoRlD</em></h1></header>")]
        public void Test009(string controlHtml)
        {
            var sut = new TextNodeComparer(ignoreCase: true);
            var rootSource = ToComparisonSource(controlHtml);
            var controlSource = new ComparisonSource(rootSource.Node.FirstChild.FirstChild.FirstChild, 0, rootSource.Path, ComparisonSourceType.Control);
            var testSource = ToComparisonSource("hello world", ComparisonSourceType.Test);
            var comparison = new Comparison(controlSource, testSource);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        }

        [Theory(DisplayName = "When diff:regex attribute is found on the immediate parent element, the control text is expected to a regex and that used when comparing to the test text node.")]
        [InlineData(@"<p diff:regex>\d{4}</p>")]
        [InlineData(@"<p diff:regex=""true"">\d{4}</p>")]
        public void Test010(string controlHtml)
        {
            var sut = new TextNodeComparer();
            var paragraphSource = ToComparisonSource(controlHtml);
            var controlSource = new ComparisonSource(paragraphSource.Node.FirstChild, 0, paragraphSource.Path, ComparisonSourceType.Control);
            var testSource = ToComparisonSource("1234", ComparisonSourceType.Test);
            var comparison = new Comparison(controlSource, testSource);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When diff:regex attribute is found on the immediate parent element and ignoreCase is true, the regex compare is done as case insensitive.")]
        public void Test011()
        {
            var sut = new TextNodeComparer(ignoreCase: true);
            var paragraphSource = ToComparisonSource(@"<p diff:regex>FOO\d{4}</p>");
            var controlSource = new ComparisonSource(paragraphSource.Node.FirstChild, 0, paragraphSource.Path, ComparisonSourceType.Control);
            var testSource = ToComparisonSource("foo1234", ComparisonSourceType.Test);
            var comparison = new Comparison(controlSource, testSource);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When diff:regex='false' attribute is found on the immediate parent element, a string ordinal case comparison is performed.")]
        [InlineData(@"<p diff:regex=""false"">1234</p>")]
        public void Test012(string controlHtml)
        {
            var sut = new TextNodeComparer();
            var paragraphSource = ToComparisonSource(controlHtml);
            var controlSource = new ComparisonSource(paragraphSource.Node.FirstChild, 0, paragraphSource.Path, ComparisonSourceType.Control);
            var testSource = ToComparisonSource("1234", ComparisonSourceType.Test);
            var comparison = new Comparison(controlSource, testSource);

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }
    }
}


