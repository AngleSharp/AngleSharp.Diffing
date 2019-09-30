using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.AngleSharp.Diffing.Comparers
{
    public class DiffIgnoreAttributeNodeFilterTest : DiffingTestBase
    {
        [Theory(DisplayName = "If control element contains ignore attribute current decision is returned")]
        [InlineData(CompareResult.Same)]
        [InlineData(CompareResult.Different)]
        [InlineData(CompareResult.SameAndBreak)]
        [InlineData(CompareResult.DifferentAndBreak)]
        public void Test00(CompareResult expectedResult)
        {
            var elm = ToComparisonSource<IElement>("<p></p>");
            var sut = new DiffIgnoreAttributeCompareStrategy();

            sut.Compare(new Comparison<IElement>(elm, elm), expectedResult).ShouldBe(expectedResult);
        }

        [Fact(DisplayName = "Ignore attribute comparer returns SameAndBreak when a control element has 'diff:ignore' attribute")]
        public void Test1()
        {
            var controlElm = ToComparisonSource<IElement>("<p diff:ignore>foo</p>");
            var testElm = ToComparisonSource<IElement>("<p></p>");
            var sut = new DiffIgnoreAttributeCompareStrategy();

            sut.Compare(new Comparison<IElement>(controlElm, testElm), CompareResult.Same).ShouldBe(CompareResult.SameAndBreak);
        }

        [Theory(DisplayName = "Ignore attribute comparer returns SameAndBreak when a control element has 'diff:ignore=trueValue' attribute")]
        [InlineData("true")]
        [InlineData("TRUE")]
        [InlineData("TrUe")]
        public void Test2(string trueValue)
        {
            var controlElm = ToComparisonSource<IElement>($@"<p diff:ignore=""{trueValue}"">foo</p>");
            var testElm = ToComparisonSource<IElement>("<p></p>");
            var sut = new DiffIgnoreAttributeCompareStrategy();

            sut.Compare(new Comparison<IElement>(controlElm, testElm), CompareResult.Same).ShouldBe(CompareResult.SameAndBreak);
        }

        [Theory(DisplayName = "Ignore attribute filter returns current decision when a control element has 'diff:ignore=falseValue' attribute with non-true value")]
        [InlineData("false")]
        [InlineData("FALSE")]
        [InlineData("FlAse")]
        [InlineData("foo")]
        public void Test3(string falseValue)
        {
            var controlElm = ToComparisonSource<IElement>($@"<p diff:ignore=""{falseValue}"">foo</p>");
            var testElm = ToComparisonSource<IElement>("<p></p>");
            var sut = new DiffIgnoreAttributeCompareStrategy();

            sut.Compare(new Comparison<IElement>(controlElm, testElm), CompareResult.Same).ShouldBe(CompareResult.Same);
        }
    }
}
