namespace AngleSharp.Diffing.Core;

public class AttributeComparisonSourceTest : DiffingTestBase
{
    public AttributeComparisonSourceTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Fact(DisplayName = "When a null is used for element source, an exception is thrown")]
    public void Test003()
    {
        Should.Throw<ArgumentNullException>(() => new AttributeComparisonSource(null!, new ComparisonSource()));
        Should.Throw<ArgumentNullException>(() => new AttributeComparisonSource("", new ComparisonSource()));
    }

    [Fact(DisplayName = "When a element source does not contain the specified attribute name, an exception is thrown")]
    public void Test004()
    {
        var elementSource = ToComparisonSource(@"<br>", ComparisonSourceType.Control);

        Should.Throw<ArgumentException>(() => new AttributeComparisonSource("notFoundAttr", elementSource));
    }

    [Fact(DisplayName = "Two sources are equal if all their properties are equal")]
    public void Test1()
    {
        var elementSource = ToComparisonSource(@"<br foo=""bar"">", ComparisonSourceType.Control);
        var source = new AttributeComparisonSource("foo", elementSource);
        var otherSource = new AttributeComparisonSource("foo", elementSource);

        source.Equals(otherSource).ShouldBeTrue();
        source.Equals((object)otherSource).ShouldBeTrue();
        (source == otherSource).ShouldBeTrue();
        (source != otherSource).ShouldBeFalse();
    }

    [Fact(DisplayName = "Two sources are not equal if their attribute is different")]
    public void Test11()
    {
        var elementSource = ToComparisonSource(@"<br foo=""bar"" bar=""baz"">", ComparisonSourceType.Control);
        var source = new AttributeComparisonSource("foo", elementSource);
        var otherSource = new AttributeComparisonSource("bar", elementSource);

        source.Equals(otherSource).ShouldBeFalse();
        (source == otherSource).ShouldBeFalse();
        (source != otherSource).ShouldBeTrue();
    }

    [Fact(DisplayName = "Two sources are not equal if their element source is different")]
    public void Test3()
    {
        var elementSource = ToComparisonSource(@"<br foo=""bar"" bar=""baz"">", ComparisonSourceType.Control);
        var otherElementSource = ToComparisonSource(@"<br foo=""bar"" bar=""baz"">", ComparisonSourceType.Control);
        var source = new AttributeComparisonSource("foo", elementSource);
        var otherSource = new AttributeComparisonSource("bar", otherElementSource);

        source.Equals(otherSource).ShouldBeFalse();
        (source == otherSource).ShouldBeFalse();
        (source != otherSource).ShouldBeTrue();
    }

    [Fact(DisplayName = "GetHashCode correctly returns same value for two equal sources")]
    public void Test001()
    {
        var elementSource = ToComparisonSource(@"<br foo=""bar"">", ComparisonSourceType.Control);
        var source = new AttributeComparisonSource("foo", elementSource);
        var otherSource = new AttributeComparisonSource("foo", elementSource);

        source.GetHashCode().ShouldBe(otherSource.GetHashCode());
    }

    [Fact(DisplayName = "GetHashCode correctly returns different values for two unequal sources")]
    public void Test002()
    {
        var elementSource = ToComparisonSource(@"<br foo=""bar"" bar=""baz"">", ComparisonSourceType.Control);
        var otherElementSource = ToComparisonSource(@"<br foo=""bar"" bar=""baz"">", ComparisonSourceType.Control);
        var source = new AttributeComparisonSource("foo", elementSource);
        var otherSource = new AttributeComparisonSource("bar", otherElementSource);

        source.GetHashCode().ShouldNotBe(otherSource.GetHashCode());
    }
}
