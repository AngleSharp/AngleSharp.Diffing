//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AngleSharp;
//using AngleSharp.Dom;
//using AngleSharp.Html.Parser;
//using Shouldly;
//using Xunit;

//namespace Egil.AngleSharp.Diffing
//{
//    public class DifferenceEngineTest : DiffingTestBase
//    {
//        private DifferenceEngine CreateSutWithDefaults()
//        {
//            var result = new DifferenceEngine();
//            result.NodeFilter = x => true; //Filters.None;
//            result.NodeMatcher = (x, y) => new Comparison<INode>();
//            result.AttributeFilter = Filters.None;
//            result.AttributeMatcher = (x, y) => new Comparison<IAttr>();
//            return result;
//        }

//        [Fact(DisplayName = "Engine uses specified NodeFilter and NodeMatcher")]
//        public void EngineUsesSpecifiedNodeFilterAndNodeMatcher()
//        {
//            var filterCalledTimes = 0;
//            var comparisons = new List<INode>();
//            var nodelist = ToNodeList("<p>foo</p><div>bar</div>");
//            var sut = CreateSutWithDefaults();
//            sut.NodeFilter = (node) => { filterCalledTimes += 1; return filterCalledTimes == 1; };
//            sut.NodeMatcher = (node, _) => { comparisons.Add(node); return new Comparison<INode>(); };

//            sut.Compare(nodelist, nodelist).ToList();

//            filterCalledTimes.ShouldBe(2);
//            comparisons.Count.ShouldBe(1);
//        }

//        [Fact(DisplayName = "Engine uses specified AttributeFilter and AttributeMatcher")]
//        public void EngineUsesAttrFilter()
//        {
//            var filterCalledTimes = 0;
//            var comparisons = new List<IAttr>();
//            var nodelist = ToNodeList(@"<p attr=""foo"" prop=""bar""></p>");
//            var sut = CreateSutWithDefaults();
//            sut.AttributeFilter = (attr) => { filterCalledTimes += 1; return filterCalledTimes == 1; };
//            sut.AttributeMatcher = (attr, _) => { comparisons.Add(attr); return new Comparison<IAttr>(); };

//            sut.Compare(nodelist, nodelist).ToList();

//            filterCalledTimes.ShouldBe(2);
//            comparisons.Count.ShouldBe(1);
//        }

//        [Fact(DisplayName = "Engine uses specified NodeComparer and AttributeComparer to produce Diffs")]
//        public void UsesNodeComparerAndAttrComparer()
//        {

//        }
//    }
//}