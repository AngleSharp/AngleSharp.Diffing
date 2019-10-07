using Egil.AngleSharp.Diffing.Core;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies
{


    public class WhitespaceStrategyTest : DiffingTestBase
    {
        public static readonly char[] AllWhitespaceCharacters = new[]
        {
            // SpaceSeparator category
            '\u0020', '\u00A0', '\u1680', '\u2000', '\u2001', '\u2002', '\u2003', '\u2004', '\u2005', '\u2006', '\u2007', '\u2008', '\u2009', '\u200A', '\u202F', '\u205F', '\u3000',
            // LineSeparator category
            '\u2028',
            //ParagraphSeparator category
            '\u2029',
            // CHARACTER TABULATION
            '\u0009','\u000A','\u000B','\u000C','\u000D','\u0085'
        };

        public static IEnumerable<object[]> WhitespaceStrings = AllWhitespaceCharacters.Select(c => new string[] { c.ToString(CultureInfo.InvariantCulture) })
            .ToArray();

        [Theory(DisplayName = "When whitespace option is Preserve, the provided decision is not changed by the filter for whitespace only text nodes")]
        [MemberData(nameof(WhitespaceStrings))]
        public void Test1(string whitespace)
        {
            var sut = new WhitespaceStrategy(WhitespaceOption.Preserve);
            var source = ToComparisonSource(whitespace);

            sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
            sut.Filter(source, FilterDecision.Exclude).ShouldBe(FilterDecision.Exclude);
        }

        [Theory(DisplayName = "When whitespace option is RemoveWhitespaceNodes, whitespace only text nodes are excluded during filtering")]
        [MemberData(nameof(WhitespaceStrings))]
        public void Test2(string whitespace)
        {
            var sut = new WhitespaceStrategy(WhitespaceOption.RemoveWhitespaceNodes);
            var source = ToComparisonSource(whitespace);

            sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
        }

        [Theory(DisplayName = "When whitespace option is Normalize, whitespace only text nodes are excluded during filtering")]
        [MemberData(nameof(WhitespaceStrings))]
        public void Test3(string whitespace)
        {
            var sut = new WhitespaceStrategy(WhitespaceOption.Normalize);
            var source = ToComparisonSource(whitespace);

            sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Exclude);
        }

        [Fact(DisplayName = "Filter method doesn't change the decision of non-whitespace nodes")]
        public void Test4()
        {
            var sut = new WhitespaceStrategy(WhitespaceOption.Normalize);
            var source = ToComparisonSource("hello world");

            sut.Filter(source, FilterDecision.Keep).ShouldBe(FilterDecision.Keep);
            sut.Filter(source, FilterDecision.Exclude).ShouldBe(FilterDecision.Exclude);
        }

        
    }
}


