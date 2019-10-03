//using AngleSharp.Dom;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Egil.AngleSharp.Diffing.Filters
//{
//    public class WhitespaceOnlyNodeFilterTest : DiffingTestBase
//    {
//        [Theory(DisplayName = "Filter returns false for any text node that only returns whitespace characters")]
//        [InlineData(" ")]
//        [InlineData("\n")]
//        [InlineData("\n\r")]
//        [InlineData("\t")]
//        [InlineData("\t\n\r")]
//        [InlineData(" \t \n\r \n ")]
//        public void Test1(string whitespace)
//        {
//            var textNode = ToComparisonSource<IText>(whitespace);
//            var sut = new WhitespaceOnlyNode();

//            sut.Filter(textNode, true).ShouldBeFalse();
//        }

//        [Fact(DisplayName = "Filter returns true when there is a none-whitespace char in a text node")]
//        public void Test2()
//        {
//            var textNode = ToComparisonSource<IText>("hello world");
//            var sut = new WhitespaceOnlyNode();

//            sut.Filter(textNode, true).ShouldBeTrue();

//        }
//    }
//}
