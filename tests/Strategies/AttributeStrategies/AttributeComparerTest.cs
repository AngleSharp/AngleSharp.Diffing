using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{

    public class AttributeComparerTest : DiffingTestBase
    {
        [Fact(DisplayName = "When compare is called with a current decision of Same or SameAndBreak, the current decision is returned")]
        public void Test001()
        {
            var sut = new AttributeComparer();
            var comparison = ToAttributeComparison(@"<b foo>", "foo",
                                                    "<b bar>", "bar");

            sut.Compare(comparison, CompareResult.Same).ShouldBe(CompareResult.Same);
            sut.Compare(comparison, CompareResult.SameAndBreak).ShouldBe(CompareResult.SameAndBreak);
        }

        [Fact(DisplayName = "When two attributes has the same name and no value, the compare result is Same")]
        public void Test002()
        {
            var sut = new AttributeComparer();
            var comparison = ToAttributeComparison(@"<b foo>", "foo",
                                                    "<b foo>", "foo");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When two attributes does not have the same name, the compare result is Different")]
        public void Test003()
        {
            var sut = new AttributeComparer();
            var comparison = ToAttributeComparison(@"<b foo>", "foo",
                                                    "<b bar>", "bar");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        }

        [Fact(DisplayName = "When two attribute values are the same, the compare result is Same")]
        public void Test004()
        {
            var sut = new AttributeComparer();
            var comparison = ToAttributeComparison(@"<b foo=""bar"">", "foo",
                                                   @"<b foo=""bar"">", "foo");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When two attribute values are different, the compare result is Different")]
        public void Test005()
        {
            var sut = new AttributeComparer();
            var comparison = ToAttributeComparison(@"<b foo=""bar"">", "foo",
                                                   @"<b foo=""baz"">", "foo");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Different);
        }

        [Fact(DisplayName = "When the control attribute is postfixed with :ignoreCase, " +
                             "a case insensitive comparison between control and test attributes is performed")]
        public void Test006()
        {
            var sut = new AttributeComparer();
            var comparison = ToAttributeComparison(@"<b foo:ignoreCase=""BAR"">", "foo:ignorecase",
                                                   @"<b foo=""bar"">", "foo");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Fact(DisplayName = "When the control attribute is postfixed with :regex, " +
                            "the control attributes value is assumed to be a regular expression and " +
                            "that is used to match against the test attributes value")]
        public void Test007()
        {
            var sut = new AttributeComparer();
            var comparison = ToAttributeComparison(@"<b foo:regex=""foobar-\d{4}"">", "foo:regex",
                                                   @"<b foo=""foobar-2000"">", "foo");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }

        [Theory(DisplayName = "When the control attribute is postfixed with :regex:ignoreCase " +
                             "or :ignoreCase:regex, the control attributes value is assumed " +
                             "to be a regular expression and that is used to do a case insensitive " +
                             "match against the test attributes value")]
        [InlineData(":regex:ignorecase")]
        [InlineData(":ignorecase:regex")]
        public void Test008(string attrNamePostfix)
        {
            var sut = new AttributeComparer();
            var controlAttrName = $"foo{attrNamePostfix}";
            var comparison = ToAttributeComparison($@"<b {controlAttrName}=""foobar-\d{{4}}"">", controlAttrName,
                                                   @"<b foo=""FOOBAR-2000"">", "foo");

            sut.Compare(comparison, CompareResult.Different).ShouldBe(CompareResult.Same);
        }
    }

}
