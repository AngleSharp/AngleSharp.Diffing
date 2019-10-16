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

    public class IgnoreAttributeComparerTest : DiffingTestBase
    {
        [Fact(DisplayName = "When a attribute does not contain have the ':ignore' postfix, the current decision is returned")]
        public void Test003()
        {
            var comparison = ToAttributeComparison(
                @"<p foo=""bar""></p>", "foo",
                @"<p foo=""bar""></p>", "foo"
            );

            IgnoreAttributeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            IgnoreAttributeComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When a attribute does contain have the ':ignore' postfix, Same is returned")]
        public void Test004()
        {
            var comparison = ToAttributeComparison(
                @"<p foo:ignore=""bar""></p>", "foo:ignore",
                @"<p foo=""baz""></p>", "foo"
            );

            IgnoreAttributeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }
    }
}
