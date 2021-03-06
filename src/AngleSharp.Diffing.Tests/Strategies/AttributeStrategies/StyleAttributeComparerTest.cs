﻿using AngleSharp.Diffing.Core;

using Shouldly;

using Xunit;

namespace AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public class StyleAttributeComparerTest : DiffingTestBase
    {
        public StyleAttributeComparerTest(DiffingTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "When attribute is not style the current decision is used")]
        public void Test001()
        {
            var comparison = ToAttributeComparison(@"<p foo=""bar"">", "foo", @"<p foo=""zab"">", "foo");
            StyleAttributeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
            StyleAttributeComparer.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            StyleAttributeComparer.Compare(comparison, CompareResult.Skip).ShouldBe(CompareResult.Skip);
        }

        [Fact(DisplayName = "When style attributes has different values then Different is returned")]
        public void Test002()
        {
            var comparison = ToAttributeComparison(@"<p style=""color: red"">", "style", @"<p style=""color: black"">", "style");
            StyleAttributeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        }

        [Fact(DisplayName = "Comparer should correctly ignore insignificant whitespace")]
        public void Test003()
        {
            var comparison = ToAttributeComparison(@"<p style=""color: red"">", "style", @"<p style=""color:red"">", "style");
            StyleAttributeComparer.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }
    }
}
