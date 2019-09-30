using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing.Filters.Chainable
{
    public class ChainedFiltersStrategyTest
    {
        [Fact(DisplayName = "When no filters are added 'true' is returned")]
        public void Test1()
        {
            var sut = new ChainedFiltersStrategy();

            sut.NodeFilter(new ComparisonSource<INode>()).ShouldBeTrue();
            sut.AttributeFilter(new AttributeComparisonSource()).ShouldBeTrue();
        }

        [Theory(DisplayName = "When one filter is added that decides the outcome")]
        [InlineData(true)]
        [InlineData(false)]
        public void Test2(bool expectedResult)
        {
            var sut = new ChainedFiltersStrategy();
            sut.AddFilter((in IComparisonSource<INode> source, bool currentDecision) => expectedResult);
            sut.AddFilter((in IAttributeComparisonSource source, bool currentDecision) => expectedResult);

            sut.NodeFilter(new ComparisonSource<INode>()).ShouldBe(expectedResult);
            sut.AttributeFilter(new AttributeComparisonSource()).ShouldBe(expectedResult);
        }

        [Theory(DisplayName = "If two or more filters are added, the last decides the outcome")]
        [InlineData(true)]
        [InlineData(false)]
        public void Test3(bool expectedResult)
        {
            var sut = new ChainedFiltersStrategy();
            sut.AddFilter((in IComparisonSource<INode> source, bool currentDecision) => !expectedResult);
            sut.AddFilter((in IComparisonSource<INode> source, bool currentDecision) => expectedResult);
            sut.AddFilter((in IAttributeComparisonSource source, bool currentDecision) => !expectedResult);
            sut.AddFilter((in IAttributeComparisonSource source, bool currentDecision) => expectedResult);

            sut.NodeFilter(new ComparisonSource<INode>()).ShouldBe(expectedResult);
            sut.AttributeFilter(new AttributeComparisonSource()).ShouldBe(expectedResult);
        }

        [Theory(DisplayName = "Filters can be specified via IChainable*FilterStrategy implmentations")]
        [InlineData(true)]
        [InlineData(false)]
        public void Test4(bool expectedResult)
        {
            var sut = new ChainedFiltersStrategy();
            sut.AddFilter(new StubNodeFilter<INode>((in IComparisonSource<INode> source, bool currentDecision) => expectedResult));
            sut.AddFilter(new StubAttrFilter((in IAttributeComparisonSource source, bool currentDecision) => expectedResult));

            sut.NodeFilter(new ComparisonSource<INode>()).ShouldBe(expectedResult);
            sut.AttributeFilter(new AttributeComparisonSource()).ShouldBe(expectedResult);
        }

        [Fact(DisplayName = "Filters should only be called with types they are compatible with")]
        public void Test5()
        {
            var elmfilterCalledTimes = 0;
            var commentfilterCalledTimes = 0;
            var textfilterCalledTimes = 0;
            var nodefilterCalledTimes = 0;
            var sut = new ChainedFiltersStrategy();
            sut.AddFilter(new StubNodeFilter<IElement>((in IComparisonSource<IElement> source, bool currentDecision) => { elmfilterCalledTimes++; return currentDecision; }));
            sut.AddFilter(new StubNodeFilter<IComment>((in IComparisonSource<IComment> source, bool currentDecision) => { commentfilterCalledTimes++; return currentDecision; }));
            sut.AddFilter(new StubNodeFilter<IText>((in IComparisonSource<IText> source, bool currentDecision) => { textfilterCalledTimes++; return currentDecision; }));
            sut.AddFilter(new StubNodeFilter<INode>((in IComparisonSource<INode> source, bool currentDecision) => { nodefilterCalledTimes++; return currentDecision; }));
            sut.AddFilter((in IComparisonSource<IElement> source, bool currentDecision) => { elmfilterCalledTimes++; return currentDecision; });
            sut.AddFilter((in IComparisonSource<IComment> source, bool currentDecision) => { commentfilterCalledTimes++; return currentDecision; });
            sut.AddFilter((in IComparisonSource<IText> source, bool currentDecision) => { textfilterCalledTimes++; return currentDecision; });
            sut.AddFilter((in IComparisonSource<INode> source, bool currentDecision) => { nodefilterCalledTimes++; return currentDecision; });

            sut.NodeFilter(new ComparisonSource<IElement>());
            sut.NodeFilter(new ComparisonSource<IComment>());
            sut.NodeFilter(new ComparisonSource<IText>());
            sut.NodeFilter(new ComparisonSource<INode>());

            elmfilterCalledTimes.ShouldBe(2);
            commentfilterCalledTimes.ShouldBe(2);
            textfilterCalledTimes.ShouldBe(2);
            nodefilterCalledTimes.ShouldBe(8);
        }

        class StubNodeFilter<TNode> : IChainableNodeFilterStrategy<TNode> where TNode : INode
        {
            private readonly ChainableNodeFilterStrategy<TNode> _filterStrategy;

            public StubNodeFilter(ChainableNodeFilterStrategy<TNode> filterStrategy)
            {
                _filterStrategy = filterStrategy;
            }

            public bool Filter(IComparisonSource<TNode> comparisonSource, bool currentDecision)
            {
                return _filterStrategy(comparisonSource, currentDecision);
            }
        }

        class StubAttrFilter : IChainableAttributeFilterStrategy
        {
            private readonly ChainableAttributeFilterStrategy _filterStrategy;

            public StubAttrFilter(ChainableAttributeFilterStrategy filterStrategy)
            {
                _filterStrategy = filterStrategy;
            }
            public bool Filter(IAttributeComparisonSource attrComparisonSource, bool currentDecision)
            {
                return _filterStrategy(attrComparisonSource, currentDecision);
            }
        }
    }
}
