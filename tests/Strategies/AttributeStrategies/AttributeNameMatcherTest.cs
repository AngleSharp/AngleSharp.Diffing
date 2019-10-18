using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public class AttributeNameMatcherTest : DiffingTestBase
    {
        private readonly DiffContext _context = new DiffContext(null, null);

        [Fact(DisplayName = "When one or both source maps is empty, no matches are returned")]
        public void Test001()
        {
            var controls = ToSourceMap("<p>", ComparisonSourceType.Control);
            var tests = ToSourceMap("<p>", ComparisonSourceType.Test);

            var actual = AttributeNameMatcher.Match(_context, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }

        [Theory(DisplayName = "When control and test sources have the same number of attributes, all should be matched")]
        [InlineData(@"<p foo=""bar"">", @"<p foo=""bar"">", 1)]
        [InlineData(@"<p baz=""bin"" foo=""bar"">", @"<p foo=""bar"" baz=""bin"">", 2)]
        [InlineData(@"<p foo=""bar"" baz=""bin"" required>", @"<p baz=""bin"" required foo=""bar"">", 3)]
        public void Test002(string controlHtml, string testHtml, int expectedMatches)
        {
            var controls = ToSourceMap(controlHtml, ComparisonSourceType.Control);
            var tests = ToSourceMap(testHtml, ComparisonSourceType.Test);

            var actual = AttributeNameMatcher.Match(_context, controls, tests).ToList();

            actual.Count.ShouldBe(expectedMatches);
            actual.ShouldAllBe(c => c.Control.Attribute.Name == c.Test.Attribute.Name);
        }

        [Theory(DisplayName = "When control and test sources have the different number of attributes, all that match should be matched")]
        [InlineData(@"<p foo=""bar"">", @"<p>", 0)]
        [InlineData(@"<p foo=""bar"">", @"<p foo=""bar"" baz=""bin"">", 1)]
        [InlineData(@"<p foo=""bar"" required>", @"<p required>", 1)]
        public void Test003(string controlHtml, string testHtml, int expectedMatches)
        {
            var controls = ToSourceMap(controlHtml, ComparisonSourceType.Control);
            var tests = ToSourceMap(testHtml, ComparisonSourceType.Test);

            var actual = AttributeNameMatcher.Match(_context, controls, tests).ToList();

            actual.Count.ShouldBe(expectedMatches);
            actual.ShouldAllBe(c => c.Control.Attribute.Name == c.Test.Attribute.Name);
        }

        [Fact(DisplayName = "WHen a control attribute has been marked as matched, it is not matched again")]
        public void Test004()
        {
            var controls = ToSourceMap(@"<p foo=""bar"">", ComparisonSourceType.Control);
            var tests = ToSourceMap(@"<p foo=""bar"">", ComparisonSourceType.Test);
            controls.MarkAsMatched(controls["foo"]);

            var actual = AttributeNameMatcher.Match(_context, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }

        [Fact(DisplayName = "WHen a test attribute has been marked as matched, it is not matched again")]
        public void Test005()
        {
            var controls = ToSourceMap(@"<p foo=""bar"">", ComparisonSourceType.Control);
            var tests = ToSourceMap(@"<p foo=""bar"">", ComparisonSourceType.Test);
            tests.MarkAsMatched(tests["foo"]);

            var actual = AttributeNameMatcher.Match(_context, controls, tests).ToList();

            actual.ShouldBeEmpty();
        }
    }
}
