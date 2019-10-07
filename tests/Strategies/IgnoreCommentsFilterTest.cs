using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies
{
    public class IgnoreCommentsFilterTest : DiffingTestBase
    {
        [Theory(DisplayName = "Comment nodes and their child node are filtered out")]
        [InlineData("<!---->")]
        [InlineData("<!-- comment -->")]
        public void Test1(string html)
        {
            var commentSource = ToComparisonSource(html);

            IgnoreCommentsFilter.Filter(commentSource, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
        }

        [Theory(DisplayName = "When called with a none-comment node, the current decision is returned")]
        [InlineData("Textnode")]
        [InlineData("<p></p>")]
        public void Test2(string html)
        {
            var source = ToComparisonSource(html);

            IgnoreCommentsFilter.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
        }
    }
}
