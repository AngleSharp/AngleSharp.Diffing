namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

public class ClassAttributeComparerTest : DiffingTestBase
{
    public ClassAttributeComparerTest(DiffingTestFixture fixture) : base(fixture)
    {
    }

    [Theory(DisplayName = "When a class attribute is compared, the order of individual " +
                          "classes and multiple whitespace is ignored")]
    [InlineData("", "")]
    [InlineData(" foo", "foo ")]
    [InlineData("foo bar", " foo    bar ")]
    [InlineData("foo bar", "bar   foo ")]
    [InlineData("foo bar baz", "bar  foo  baz  ")]
    public void Test009(string controlClasses, string testClasses)
    {
        var comparison = ToAttributeComparison($@"<p class=""{controlClasses}"">", "class",
                                               $@"<p class=""{testClasses}"">", "class");

        ClassAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Same);
    }

    [Fact(DisplayName = "When a class attribute is matched up with another attribute, the result is different")]
    public void Test010()
    {
        var comparison = ToAttributeComparison(@"<p class=""foo"">", "class",
                                               @"<p bar=""bar"">", "bar");

        ClassAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Different);
    }

    [Theory(DisplayName = "When there are different number of classes in the class attributes the result is different")]
    [InlineData("foo bar baz", "baz foo")]
    [InlineData("bar baz", "bar baz foo")]
    public void Test011(string controlClasses, string testClasses)
    {
        var comparison = ToAttributeComparison($@"<p class=""{controlClasses}"">", "class",
                                               $@"<p class=""{testClasses}"">", "class");

        ClassAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Different);
    }

    [Theory(DisplayName = "When the classes in the class attributes are different the result is different")]
    [InlineData("foo", "bar")]
    [InlineData("foo bar", "baz bin")]
    [InlineData("foo bar", "foo bin")]
    [InlineData("foo bar", "baz bar")]
    public void Test012(string controlClasses, string testClasses)
    {
        var comparison = ToAttributeComparison($@"<p class=""{controlClasses}"">", "class",
                                               $@"<p class=""{testClasses}"">", "class");

        ClassAttributeComparer.Compare(comparison, CompareResult.Unknown).ShouldBe(CompareResult.Different);
    }
}
