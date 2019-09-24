using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
    public class ElementIgnoreAttributeFiltersTest : DiffingTestBase
    {
        [Fact(DisplayName = "Ignore attribute filter returns false for elements with 'diff:ignore' attribute")]
        public void IgnoreAttributeFilterReturnsFalseForElementsWithEmptyAttr()
        {
            var node = ToNode(@"<p diff:ignore>foo</p>");
            Filters.ElementIgnoreAttributeFilter(node).ShouldBeFalse();
        }

        [Theory(DisplayName = "Ignore attribute filter returns false for elements with 'diff:ignore=trueValue' attribute")]
        [InlineData("true")]
        [InlineData("TRUE")]
        [InlineData("TrUe")]
        public void IgnoreAttributeFilterReturnsFalseForElementsWithAttrTrueValue(string trueValue)
        {
            var node = ToNode($@"<p diff:ignore=""{trueValue}"">foo</p>");
            Filters.ElementIgnoreAttributeFilter(node).ShouldBeFalse();
        }

        [Theory(DisplayName = "Ignore attribute filter returns true for elements with 'diff:ignore=falseValue' attribute with non-true value")]
        [InlineData("false")]
        [InlineData("FALSE")]
        [InlineData("FlAse")]
        [InlineData("foo")]
        public void IgnoreAttributeFilterReturnsTrueForElementsWithAttrTrueValue(string falseValue)
        {
            var node = ToNode($@"<p diff:ignore=""{falseValue}"">foo</p>");
            Filters.ElementIgnoreAttributeFilter(node).ShouldBeTrue();
        }
    }
}