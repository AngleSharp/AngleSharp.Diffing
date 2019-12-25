using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace AngleSharp.Diffing.Core
{
    public class SourceCollectionTest : DiffingTestBase
    {
        public SourceCollectionTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Theory(DisplayName = "Source type is correct saved in the collection")]
        [InlineData(ComparisonSourceType.Control)]
        [InlineData(ComparisonSourceType.Test)]
        public void Test0(ComparisonSourceType sourceType)
        {
            var sut = new SourceCollection(sourceType, Array.Empty<ComparisonSource>());

            sut.SourceType.ShouldBe(sourceType);
        }

        [Fact(DisplayName = "When adding sources are added to the collection, they are accessible through the indexer")]
        public void Test1()
        {
            var sut = ToNodeList("<p></p><span></span>").ToSourceCollection(ComparisonSourceType.Control);

            sut.Count.ShouldBe(2);
            sut.First().ShouldSatisfyAllConditions(
                (ComparisonSource cs) => cs.Index.ShouldBe(0),
                (ComparisonSource cs) => cs.Node.NodeName.ShouldBe("P"));
            sut.Last().ShouldSatisfyAllConditions(
                (ComparisonSource cs) => cs.Index.ShouldBe(1),
                (ComparisonSource cs) => cs.Node.NodeName.ShouldBe("SPAN"));
        }

        [Fact(DisplayName = "When a source is marked as matched, it isnt included in output of GetUnmatched")]
        public void Test2()
        {
            var sut = ToNodeList("<p></p><span></span>").ToSourceCollection(ComparisonSourceType.Control);
            var first = sut.First();
            var last = sut.Last();

            sut.MarkAsMatched(first);

            sut.GetUnmatched().Count().ShouldBe(1);
            sut.GetUnmatched().First().ShouldBe(last);
            sut.Count.ShouldBe(2);
        }

        [Fact(DisplayName = "Collection throws if sources is not ordered source-index when enumerated")]
        public void Test3()
        {
            var sources = ToNodeList("<span></span><p></p><main></main><h1></h1><ul></ul><ol></ol><em></em>")
                .ToComparisonSourceList(ComparisonSourceType.Control)
                .OrderBy(x => x.Node.NodeName);

            var cut = new SourceCollection(ComparisonSourceType.Control, sources);

            Should.Throw<InvalidOperationException>(() => cut.ToList());
            Should.Throw<InvalidOperationException>(() => cut.GetAllSources().ToList());
        }

        [Fact(DisplayName = "When an source is removed from the list, it does not add to the count nor is it returned when iterating")]
        public void Test4()
        {
            var sut = ToNodeList("<p></p><span></span>").ToSourceCollection(ComparisonSourceType.Control);
            var first = sut.First();
            var last = sut.Last();

            sut.Remove((in ComparisonSource x) => x == first ? FilterDecision.Exclude : FilterDecision.Keep);

            sut.Count.ShouldBe(1);
            sut.First().ShouldBe(last);
        }

        [Fact(DisplayName = "Marking removed source as matched throws")]
        public void Test7()
        {
            var sut = ToNodeList("<p></p><span></span>").ToSourceCollection(ComparisonSourceType.Control);
            var last = sut.Last();
            sut.Remove((in ComparisonSource x) => x == last ? FilterDecision.Exclude : FilterDecision.Keep);

            Should.Throw<InvalidOperationException>(() => sut.MarkAsMatched(last));
        }
    }
}
