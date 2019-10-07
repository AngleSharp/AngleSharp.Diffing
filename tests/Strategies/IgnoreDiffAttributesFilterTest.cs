using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies
{
    public class IgnoreDiffAttributesFilterTest : DiffingTestBase
    {
        [Theory(DisplayName = "When an attribute starts with 'diff:' it is filtered out")]
        [InlineData(@"<p diff:whitespace=""Normalize"">", "diff:whitespace")]
        [InlineData(@"<p diff:ignore=""true"">", "diff:ignore")]
        public void Test1(string elementHtml, string diffAttrName)
        {
            var elmSource = ToComparisonSource(elementHtml);
            var attr = ((IElement)elmSource.Node).Attributes[diffAttrName];
            var source = new AttributeComparisonSource(attr, elmSource);

            IgnoreDiffAttributesFilter.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
        }

        [Theory(DisplayName = "When an attribute does not starts with 'diff:' its current decision is used")]
        [InlineData(@"<p lang=""csharp"">", "lang")]
        [InlineData(@"<p diff=""foo"">", "diff")]
        [InlineData(@"<p diffx=""foo"">", "diffx")]
        public void Test2(string elementHtml, string diffAttrName)
        {
            var elmSource = ToComparisonSource(elementHtml);
            var attr = ((IElement)elmSource.Node).Attributes[diffAttrName];
            var source = new AttributeComparisonSource(attr, elmSource);

            IgnoreDiffAttributesFilter.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
        }
    }
}
