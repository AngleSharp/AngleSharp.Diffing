using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies.IgnoreStrategies
{
    public class InlineIgnoreComparerTest : DiffingTestBase
    {
        [Theory(DisplayName = "When a control element does not contain the 'diff:ignore' attribute or it is 'diff:ignore=false', the current decision is returned")]
        [InlineData(@"<p></p>")]
        [InlineData(@"<p diff:ignore=""false""></p>")]
        [InlineData(@"<p diff:ignore=""FALSE""></p>")]
        [InlineData(@"<p diff:ignore=""faLsE""></p>")]
        public void Test001(string controlHtml)
        {
            var comparison = ToComparison(controlHtml, "<p></p>");

            InlineIgnoreComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            InlineIgnoreComparer.Compare(comparison, CompareResult.DifferentAndBreak).ShouldBe(CompareResult.DifferentAndBreak);
            InlineIgnoreComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            InlineIgnoreComparer.Compare(comparison, CompareResult.SameAndBreak).ShouldBe(CompareResult.SameAndBreak);
        }

        [Theory(DisplayName = "When a control element has 'diff:ignore' attribute, SameAndBreak is returned")]
        [InlineData(@"<p diff:ignore></p>")]
        [InlineData(@"<p diff:ignore=""true""></p>")]
        [InlineData(@"<p diff:ignore=""TRUE""></p>")]
        [InlineData(@"<p diff:ignore=""TrUe""></p>")]
        public void Test002(string controlHtml)
        {
            var comparison = ToComparison(controlHtml, "<p></p>");

            InlineIgnoreComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.SameAndBreak);
        }

        [Fact(DisplayName = "When a attribute does not contain have the ':ignore' postfix, the current decision is returned")]
        public void Test003()
        {
            var comparison = ToAttributeComparison(
                @"<p foo=""bar""></p>", "foo", 
                @"<p foo=""bar""></p>", "foo"
            );

            InlineIgnoreComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            InlineIgnoreComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When a attribute does contain have the ':ignore' postfix, Same is returned")]
        public void Test004()
        {
            var comparison = ToAttributeComparison(
                @"<p foo:ignore=""bar""></p>", "foo:ignore",
                @"<p foo=""baz""></p>", "foo"
            );

            InlineIgnoreComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }
    }
}
