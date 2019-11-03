using Shouldly;
using Xunit;

namespace AngleSharp.Diffing.Core
{
    public class ComparisonTest : DiffingTestBase
    {
        public ComparisonTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "When Control and Test properties are equal, the equal tests returns true")]
        public void Test001()
        {
            var control = ToComparisonSource(@"<p>");
            var test = ToComparisonSource(@"<p>");
            var comparison = new Comparison(control, test);
            var otherComparison = new Comparison(control, test);

            comparison.Equals(otherComparison).ShouldBeTrue();
            comparison.Equals((object)otherComparison).ShouldBeTrue();
            (comparison == otherComparison).ShouldBeTrue();
            (comparison != otherComparison).ShouldBeFalse();
        }

        [Fact(DisplayName = "When Control and Test properties not equal, the equal tests returns false")]
        public void Test002()
        {
            var source1 = ToComparisonSource(@"<p>");
            var source2 = ToComparisonSource(@"<p>");
            var comparison = new Comparison(source1, source1);
            var otherComparison = new Comparison(source2, source2);

            comparison.Equals(otherComparison).ShouldBeFalse();
            comparison.Equals((object)otherComparison).ShouldBeFalse();
            (comparison == otherComparison).ShouldBeFalse();
            (comparison != otherComparison).ShouldBeTrue();
        }

        [Fact(DisplayName = "GetHashCode correctly returns same value for two equal sources")]
        public void Test003()
        {
            var control = ToComparisonSource(@"<p>");
            var test = ToComparisonSource(@"<p>");
            var comparison = new Comparison(control, test);
            var otherComparison = new Comparison(control, test);

            comparison.GetHashCode().ShouldBe(otherComparison.GetHashCode());
        }

        [Fact(DisplayName = "GetHashCode correctly returns different values for two unequal sources")]
        public void Test004()
        {
            var source1 = ToComparisonSource(@"<p>");
            var source2 = ToComparisonSource(@"<p>");
            var comparison = new Comparison(source1, source1);
            var otherComparison = new Comparison(source2, source2);

            comparison.GetHashCode().ShouldNotBe(otherComparison.GetHashCode());
        }

        // AreNodeTypesEqual

        [Fact(DisplayName = "GetAttributeElements returns the elements in the attribute comparison")]
        public void Test005()
        {
            var control = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var test = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var comparison = new AttributeComparison(control, test);

            var (actualCtrlElm, actualTestElm) = comparison.GetAttributeElements();

            actualCtrlElm.ShouldBe(control.ElementSource.Node);
            actualTestElm.ShouldBe(test.ElementSource.Node);
        }
    }

    public class AttributeComparisonTest : DiffingTestBase
    {
        public AttributeComparisonTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "When Control and Test properties are equal, the equal tests returns true")]
        public void Test001()
        {
            var control = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var test = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var comparison = new AttributeComparison(control, test);
            var otherComparison = new AttributeComparison(control, test);

            comparison.Equals(otherComparison).ShouldBeTrue();
            comparison.Equals((object)otherComparison).ShouldBeTrue();
            (comparison == otherComparison).ShouldBeTrue();
            (comparison != otherComparison).ShouldBeFalse();
        }

        [Fact(DisplayName = "When Control and Test properties not equal, the equal tests returns false")]
        public void Test002()
        {
            var source1 = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var source2 = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var comparison = new AttributeComparison(source1, source1);
            var otherComparison = new AttributeComparison(source2, source2);

            comparison.Equals(otherComparison).ShouldBeFalse();
            comparison.Equals((object)otherComparison).ShouldBeFalse();
            (comparison == otherComparison).ShouldBeFalse();
            (comparison != otherComparison).ShouldBeTrue();
        }

        [Fact(DisplayName = "GetHashCode correctly returns same value for two equal sources")]
        public void Test003()
        {
            var control = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var test = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var comparison = new AttributeComparison(control, test);
            var otherComparison = new AttributeComparison(control, test);

            comparison.GetHashCode().ShouldBe(otherComparison.GetHashCode());
        }

        [Fact(DisplayName = "GetHashCode correctly returns different values for two unequal sources")]
        public void Test004()
        {
            var source1 = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var source2 = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var comparison = new AttributeComparison(source1, source1);
            var otherComparison = new AttributeComparison(source2, source2);

            comparison.GetHashCode().ShouldNotBe(otherComparison.GetHashCode());
        }

        [Fact(DisplayName = "GetAttributeElements returns the elements in the attribute comparison")]
        public void Test005()
        {
            var control = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var test = ToAttributeComparisonSource(@"<br foo=""bar"">", "foo");
            var comparison = new AttributeComparison(control, test);

            var (actualCtrlElm, actualTestElm) = comparison.GetAttributeElements();

            actualCtrlElm.ShouldBe(control.ElementSource.Node);
            actualTestElm.ShouldBe(test.ElementSource.Node);
        }
    }
}
