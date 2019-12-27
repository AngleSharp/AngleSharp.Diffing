using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public class PostfixedAttributeMatcherTest : DiffingTestBase
    {
        public PostfixedAttributeMatcherTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        public static IEnumerable<object[]> AttributeDiffPostfixesCombinations { get; } = new List<string[]>
        {
            new []{":ignore"},
            new []{":regex"},
            new []{":ignorecase"},
            new []{":ignorecase:regex"},
            new []{":regex:ignorecase"},
        };

        [Fact(DisplayName = "When a attribute does not have a diff postfix, no matching is performed")]
        public void Test001()
        {
            var controls = ToSourceMap(@"<p foo>", ComparisonSourceType.Control);
            var tests = ToSourceMap(@"<p foo>", ComparisonSourceType.Test);

            var actual = PostfixedAttributeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }

        [Theory(DisplayName = "When a control attribute does have a diff postfix, but has already been matched, no new match is made")]
        [MemberData(nameof(AttributeDiffPostfixesCombinations))]
        public void Test002(string diffPostfix)
        {
            var controls = ToSourceMap($@"<p foo{diffPostfix}>", ComparisonSourceType.Control);
            var tests = ToSourceMap(@"<p foo>", ComparisonSourceType.Test);
            controls.MarkAsMatched(controls[$"foo{diffPostfix}"]);

            var actual = PostfixedAttributeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }

        [Theory(DisplayName = "When a control attribute does have a diff postfix, but the test attribute has already been matched, no new match is made")]
        [MemberData(nameof(AttributeDiffPostfixesCombinations))]
        public void Test003(string diffPostfix)
        {
            var controls = ToSourceMap($@"<p foo{diffPostfix}>", ComparisonSourceType.Control);
            var tests = ToSourceMap(@"<p foo>", ComparisonSourceType.Test);
            tests.MarkAsMatched(tests["foo"]);

            var actual = PostfixedAttributeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }

        [Theory(DisplayName = "When a attribute does have a diff postfix, it is matched with a attribute with the same suffix")]
        [MemberData(nameof(AttributeDiffPostfixesCombinations))]
        public void Test004(string diffPostfix)
        {
            var controls = ToSourceMap($@"<p foo{diffPostfix}>", ComparisonSourceType.Control);
            var tests = ToSourceMap(@"<p foo>", ComparisonSourceType.Test);

            var actual = PostfixedAttributeMatcher.Match(DummyContext, controls, tests).ToList();

            actual.Count.ShouldBe(1);
            actual[0].ShouldSatisfyAllConditions(
                c => c.Control.Attribute.Name.ShouldBe($"foo{diffPostfix}"),
                c => c.Test.Attribute.Name.ShouldBe("foo")
            );
        }
    }
}
