//using AngleSharp.Dom;
//using Egil.AngleSharp.Diffing.Comparisons;
//using Shouldly;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Egil.AngleSharp.Diffing.Comparers.Chainable
//{
//    public class ChainedCompareStrategyTest
//    {
//        [Fact(DisplayName = "When no comparers are added 'Same' is returned")]
//        public void Test1()
//        {
//            var sut = new ChainedCompareStrategy();

//            sut.Compare(new Comparisons.Comparison<INode>()).ShouldBe(CompareResult.Same);
//            sut.Compare(new AttributeComparison()).ShouldBe(CompareResult.Same);
//        }

//        [Theory(DisplayName = "When one comparer is added that decides the outcome")]
//        [InlineData(CompareResult.Same)]
//        [InlineData(CompareResult.Different)]
//        [InlineData(CompareResult.SameAndBreak)]
//        [InlineData(CompareResult.DifferentAndBreak)]
//        public void Test2(CompareResult expectedCompareResult)
//        {
//            var sut = new ChainedCompareStrategy();
//            sut.AddComparer((in IComparison<INode> comparison, CompareResult currentDecision) => expectedCompareResult);
//            sut.AddComparer((in IAttributeComparison comparison, CompareResult currentDecision) => expectedCompareResult);

//            sut.Compare(new Comparison<INode>()).ShouldBe(expectedCompareResult);
//            sut.Compare(new AttributeComparison()).ShouldBe(expectedCompareResult);
//        }

//        [Theory(DisplayName = "If two or more comparers are added, the last decides the outcome")]
//        [InlineData(CompareResult.Different, CompareResult.Same)]
//        [InlineData(CompareResult.Same, CompareResult.Different)]
//        [InlineData(CompareResult.Different, CompareResult.SameAndBreak)]
//        [InlineData(CompareResult.Same, CompareResult.DifferentAndBreak)]
//        public void Test3(CompareResult firstResult, CompareResult expectedResult)
//        {
//            var sut = new ChainedCompareStrategy();
//            sut.AddComparer((in IComparison<INode> comparison, CompareResult currentDecision) => firstResult);
//            sut.AddComparer((in IComparison<INode> comparison, CompareResult currentDecision) => expectedResult);
//            sut.AddComparer((in IAttributeComparison comparison, CompareResult currentDecision) => firstResult);
//            sut.AddComparer((in IAttributeComparison comparison, CompareResult currentDecision) => expectedResult);

//            sut.Compare(new Comparison<INode>()).ShouldBe(expectedResult);
//            sut.Compare(new AttributeComparison()).ShouldBe(expectedResult);
//        }

//        [Theory(DisplayName = "Comparers can be specified via IChainable*CompareStrategy implmentations")]
//        [InlineData(CompareResult.Same)]
//        [InlineData(CompareResult.Different)]
//        [InlineData(CompareResult.SameAndBreak)]
//        [InlineData(CompareResult.DifferentAndBreak)]
//        public void Test4(CompareResult expectedResult)
//        {
//            var sut = new ChainedCompareStrategy();
//            sut.AddComparer(new StubNodeComparer<INode>((in IComparison<INode> comparison, CompareResult currentDecision) => expectedResult));
//            sut.AddComparer(new StubAttrComparer((in IAttributeComparison comparison, CompareResult currentDecision) => expectedResult));

//            sut.Compare(new Comparison<INode>()).ShouldBe(expectedResult);
//            sut.Compare(new AttributeComparison()).ShouldBe(expectedResult);
//        }

//        [Fact(DisplayName = "Comparers should only be called with types they are compatible with")]
//        public void Test5()
//        {
//            var elmCompCalledTimes = 0;
//            var commentCompCalledTimes = 0;
//            var textCompCalledTimes = 0;
//            var nodeCompCalledTimes = 0;
//            var sut = new ChainedCompareStrategy();
//            sut.AddComparer(new StubNodeComparer<IElement>((in IComparison<IElement> comparison, CompareResult currentDecision) => { elmCompCalledTimes++; return currentDecision; }));
//            sut.AddComparer(new StubNodeComparer<IComment>((in IComparison<IComment> comparison, CompareResult currentDecision) => { commentCompCalledTimes++; return currentDecision; }));
//            sut.AddComparer(new StubNodeComparer<IText>((in IComparison<IText> comparison, CompareResult currentDecision) => { textCompCalledTimes++; return currentDecision; }));
//            sut.AddComparer(new StubNodeComparer<INode>((in IComparison<INode> comparison, CompareResult currentDecision) => { nodeCompCalledTimes++; return currentDecision; }));
//            sut.AddComparer((in IComparison<IElement> comparison, CompareResult currentDecision) => { elmCompCalledTimes++; return currentDecision; });
//            sut.AddComparer((in IComparison<IComment> comparison, CompareResult currentDecision) => { commentCompCalledTimes++; return currentDecision; });
//            sut.AddComparer((in IComparison<IText> comparison, CompareResult currentDecision) => { textCompCalledTimes++; return currentDecision; });
//            sut.AddComparer((in IComparison<INode> comparison, CompareResult currentDecision) => { nodeCompCalledTimes++; return currentDecision; });

//            sut.Compare(new Comparison<IElement>());
//            sut.Compare(new Comparison<IComment>());
//            sut.Compare(new Comparison<IText>());
//            sut.Compare(new Comparison<INode>());

//            elmCompCalledTimes.ShouldBe(2);
//            commentCompCalledTimes.ShouldBe(2);
//            textCompCalledTimes.ShouldBe(2);
//            nodeCompCalledTimes.ShouldBe(8);
//        }

//        class StubNodeComparer<TNode> : IChainableNodeCompareStrategy<TNode> where TNode : INode
//        {
//            private readonly ChainableNodeComparerStrategy<TNode> _strategy;

//            public StubNodeComparer(ChainableNodeComparerStrategy<TNode> strategy)
//            {
//                _strategy = strategy;
//            }

//            public CompareResult Compare(IComparison<TNode> comparison, CompareResult currentDecision)
//            {
//                return _strategy(comparison, currentDecision);
//            }
//        }

//        class StubAttrComparer : IChainableAttributeCompareStrategy
//        {
//            private readonly ChainableAttributeComparerStrategy _strategy;

//            public StubAttrComparer(ChainableAttributeComparerStrategy strategy)
//            {
//                _strategy = strategy;
//            }

//            public CompareResult Compare(IAttributeComparison comparison, CompareResult currentDecision)
//            {
//                return _strategy(comparison, currentDecision);
//            }
//        }
//    }
//}
