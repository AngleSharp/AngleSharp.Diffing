using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Core
{
    public class ComparisonSourceTest : DiffingTestBase
    {
        public ComparisonSourceTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "Two sources are equal if all their properties are equal")]
        public void Test1()
        {
            var node =  ToNode("<br>");
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
            var source = new ComparisonSource(ToNode("<br>"), 1, "path", ComparisonSourceType.Control);
            var otherSource = new ComparisonSource(ToNode("<p>"), 1, "path", ComparisonSourceType.Control);

            source.Equals(otherSource).ShouldBeFalse();
            (source == otherSource).ShouldBeFalse();
            (source != otherSource).ShouldBeTrue();
        }
    }
}
