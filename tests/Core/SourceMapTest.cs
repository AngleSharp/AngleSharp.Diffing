using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Core
{
    public class SourceMapTest : DiffingTestBase
    {
        private ComparisonSource CreateSource(string html, ComparisonSourceType sourceType = ComparisonSourceType.Control)
        {
            return ToNode(html).ToComparisonSource(0, sourceType);
        }

        [Fact(DisplayName = "When initialized with a non-element an exception is thrown")]
        public void Test0()
        {
            var source = CreateSource(@"textnode", ComparisonSourceType.Test);

            Should.Throw<ArgumentException>(() => new SourceMap(source));
        }

        [Fact(DisplayName = "When initializing with an element source, its source type is used and its attributes are added at sources to the map")]
        public void Test1()
        {
            var elementSource = CreateSource(@"<p foo=""bar"" baz=""foo""></p>", ComparisonSourceType.Test);
            var sut = new SourceMap(elementSource);

            sut.SourceType.ShouldBe(ComparisonSourceType.Test);
            sut.Count.ShouldBe(2);
            sut["foo"].ShouldSatisfyAllConditions(
                (cs) => cs.Attribute.Name.ShouldBe("foo"),
                (cs) => cs.ElementSource.ShouldBe(elementSource)
            );
            sut["baz"].ShouldSatisfyAllConditions(
                (cs) => cs.Attribute.Name.ShouldBe("baz"),
                (cs) => cs.ElementSource.ShouldBe(elementSource)
            );
        }

        [Fact(DisplayName = "Accessing the indexer with a attr name that is not in the set throws")]
        public void Test2()
        {
            var elementSource = CreateSource(@"<p></p>");
            var sut = new SourceMap(elementSource);

            Should.Throw<ArgumentException>(() => sut["foo"])
                .Message.ShouldBe("The map does not contain an attribute comparison source that matches the name 'foo'.");
        }

        [Theory(DisplayName = "Contains returns true when map contains a source with the provided name, false otherwise")]
        [InlineData(@"<p foo=""bar"">", true)]
        [InlineData(@"<p>", false)]
        public void Tests3(string html, bool expectedResult)
        {
            var elementSource = CreateSource(html);
            var sut = new SourceMap(elementSource);

            sut.Contains("foo").ShouldBe(expectedResult);
        }

        [Fact(DisplayName = "Sources can be removed from map by passing a predicate to the Remove method")]
        public void Test4()
        {
            var elementSource = CreateSource(@"<p foo=""bar"" baz=""foo""></p>");
            var sut = new SourceMap(elementSource);

            sut.Remove((in AttributeComparisonSource cs) => cs.Attribute.Name == "foo" ? FilterDecision.Exclude : FilterDecision.Keep);

            sut.Count.ShouldBe(1);
            sut.Contains("foo").ShouldBeFalse();
        }

        [Fact(DisplayName = "When a source is marked as matched, it is not returned in GetUnmatched")]
        public void Test5()
        {
            var elementSource = CreateSource(@"<p foo=""bar"" baz=""foo""></p>");
            var sut = new SourceMap(elementSource);
            var foo = sut["foo"];

            sut.MarkAsMatched(foo);

            sut.GetUnmatched().ShouldNotContain(foo);
            sut.GetUnmatched().Count().ShouldBe(1);
        }
    }
}
