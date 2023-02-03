using Shouldly;

using Xunit;

namespace AngleSharp.Diffing.Core
{
    public class ComparisonSourceTest : DiffingTestBase
    {
        public ComparisonSourceTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "Two sources are equal if all their properties are equal")]
        public void Test1()
        {
            var node = ToNode("<br>");
            var source = new ComparisonSource(node, 1, "path", ComparisonSourceType.Control);
            var otherSource = new ComparisonSource(node, 1, "path", ComparisonSourceType.Control);

            source.Equals(otherSource).ShouldBeTrue();
            source.Equals((object)otherSource).ShouldBeTrue();
            (source == otherSource).ShouldBeTrue();
            (source != otherSource).ShouldBeFalse();
        }

        [Theory(DisplayName = "Two sources are not equal if one of their properties are not equal")]
        [InlineData(2, "path", ComparisonSourceType.Control)]
        [InlineData(1, "otherPath", ComparisonSourceType.Control)]
        [InlineData(1, "path", ComparisonSourceType.Test)]
        public void Test11(int otherIndex, string otherPath, ComparisonSourceType otherSourceType)
        {
            var node = ToNode("<br>");
            var source = new ComparisonSource(node, 1, "path", ComparisonSourceType.Control);
            var otherSource = new ComparisonSource(node, otherIndex, otherPath, otherSourceType);

            source.Equals(otherSource).ShouldBeFalse();
            (source == otherSource).ShouldBeFalse();
            (source != otherSource).ShouldBeTrue();
        }

        [Fact(DisplayName = "Two sources are not equal if their nodes are not equal")]
        public void Test3()
        {
            var source = new ComparisonSource(ToNode("<p>"), 1, "path", ComparisonSourceType.Control);
            var otherSource = new ComparisonSource(ToNode("<p>"), 1, "path", ComparisonSourceType.Control);

            source.Equals(otherSource).ShouldBeFalse();
            (source == otherSource).ShouldBeFalse();
            (source != otherSource).ShouldBeTrue();
        }

        [Fact(DisplayName = "GetHashCode correctly returns same value for two equal sources")]
        public void Test001()
        {
            var node = ToNode("<br>");
            var source = new ComparisonSource(node, 1, "path", ComparisonSourceType.Control);
            var otherSource = new ComparisonSource(node, 1, "path", ComparisonSourceType.Control);

            source.GetHashCode().ShouldBe(otherSource.GetHashCode());
        }
        [Fact(DisplayName = "GetHashCode correctly returns different values for two unequal sources")]
        public void Test002()
        {
            var source = new ComparisonSource(ToNode("<br>"), 1, "path", ComparisonSourceType.Control);
            var otherSource = new ComparisonSource(ToNode("<p>"), 2, "path/other", ComparisonSourceType.Test);

            source.GetHashCode().ShouldNotBe(otherSource.GetHashCode());
        }

        [Theory(DisplayName = "The index in the source's path is based on its position in it's parents" +
            "child node list")]
        [InlineData("<p>", 0, "p(0)")]
        [InlineData("text<p>", 1, "p(1)")]
        [InlineData("<!--x--><p>", 1, "p(1)")]
        [InlineData("<i></i>text<p>", 2, "p(2)")]
        [InlineData("<i></i><!--x--><p>", 2, "p(2)")]
        [InlineData("<i></i>text<!--x--><p>text", 2, "#comment(2)")]
        public void Test005(string sourceMarkup, int nodeIndex, string expectedPath)
        {
            var node = ToNodeList(sourceMarkup)[nodeIndex];

            var sut = new ComparisonSource(node, ComparisonSourceType.Control);

            sut.Path.ShouldBe(expectedPath);
        }

        [Fact(DisplayName = "The parent path is calculated correctly when not provided")]
        public void Test006()
        {
            var nodes = ToNodeList("<p>txt<br/><i>text</i></p>");
            var textNode = nodes[0].ChildNodes[2].FirstChild;

            var sut = new ComparisonSource(textNode, ComparisonSourceType.Control);

            sut.Path.ShouldBe("p(0) > i(2) > #text(0)");
        }

        [Fact(DisplayName = "Source uses parent path if provided to construct own path")]
        public void Test007()
        {
            var node = ToNode("<p>");
            var parentPath = "SOME > PAHT";

            var sut = new ComparisonSource(node, 0, parentPath, ComparisonSourceType.Control);

            var expectedPath = ComparisonSource.CombinePath(parentPath, ComparisonSource.GetNodePathSegment(node));
            sut.Path.ShouldBe(expectedPath);
        }
    }
}
