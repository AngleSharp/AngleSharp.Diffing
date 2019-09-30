using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Filters.Chainable
{
    public class ChainedFiltersStrategy : IFilterStrategy
    {
        private readonly List<IChainableNodeFilterStrategy<IElement>> _elementFilters = new List<IChainableNodeFilterStrategy<IElement>>();
        private readonly List<IChainableNodeFilterStrategy<IComment>> _commentFilters = new List<IChainableNodeFilterStrategy<IComment>>();
        private readonly List<IChainableNodeFilterStrategy<IText>> _textFilters = new List<IChainableNodeFilterStrategy<IText>>();
        private readonly List<IChainableNodeFilterStrategy<INode>> _nodeFilters = new List<IChainableNodeFilterStrategy<INode>>();
        private readonly List<IChainableAttributeFilterStrategy> _attrFilters = new List<IChainableAttributeFilterStrategy>();

        public bool NodeFilter(in IComparisonSource<INode> comparisonSource)
        {
            return comparisonSource switch
            {
                IComparisonSource<IElement> source => ApplyNodeFilters(in source, _elementFilters),
                IComparisonSource<IComment> source => ApplyNodeFilters(in source, _commentFilters),
                IComparisonSource<IText> source => ApplyNodeFilters(in source, _textFilters),
                IComparisonSource<INode> source => ApplyNodeFilters(in source, _nodeFilters)
            };
        }

        public bool AttributeFilter(in IAttributeComparisonSource attributeComparisonSource)
        {
            var result = true;
            foreach (var filter in _attrFilters)
            {
                result = filter.Filter(attributeComparisonSource, result);
            }
            return result;
        }

        private bool ApplyNodeFilters<TNode>(in IComparisonSource<TNode> comparisonSource, List<IChainableNodeFilterStrategy<TNode>> filters)
            where TNode : INode
        {
            var result = true;

            foreach (var filter in filters)
            {
                result = filter.Filter(comparisonSource, result);
            }

            return result;
        }

        public void AddFilter<TNode>(ChainableNodeFilterStrategy<TNode> nodeFilter)
            where TNode : INode
        {
            switch (nodeFilter)
            {
                case ChainableNodeFilterStrategy<IElement> filter:
                    AddFilter(new NodeFilterDelegateWrapper<IElement>(filter));
                    break;
                case ChainableNodeFilterStrategy<IComment> filter:
                    AddFilter(new NodeFilterDelegateWrapper<IComment>(filter));
                    break;
                case ChainableNodeFilterStrategy<IText> filer:
                    AddFilter(new NodeFilterDelegateWrapper<IText>(filer));
                    break;
                case ChainableNodeFilterStrategy<INode> filer:
                    AddFilter(new NodeFilterDelegateWrapper<INode>(filer));
                    break;
            }
        }

        public void AddFilter(IChainableNodeFilterStrategy<IElement> filterStrategy) => _elementFilters.Add(filterStrategy);
        public void AddFilter(IChainableNodeFilterStrategy<IComment> filterStrategy) => _commentFilters.Add(filterStrategy);
        public void AddFilter(IChainableNodeFilterStrategy<IText> filterStrategy) => _textFilters.Add(filterStrategy);
        public void AddFilter(IChainableNodeFilterStrategy<INode> filterStrategy)
        {
            _elementFilters.Add(filterStrategy);
            _commentFilters.Add(filterStrategy);
            _textFilters.Add(filterStrategy);
            _nodeFilters.Add(filterStrategy);
        }

        public void AddFilter(ChainableAttributeFilterStrategy attributeFilter)
        {
            AddFilter(new AttrFilterDelegateWrapper(attributeFilter));
        }

        public void AddFilter(IChainableAttributeFilterStrategy attributeFilterStrategy)
        {
            _attrFilters.Add(attributeFilterStrategy);
        }
    }

    class NodeFilterDelegateWrapper<TNode> : IChainableNodeFilterStrategy<TNode> where TNode : INode
    {
        private readonly ChainableNodeFilterStrategy<TNode> _filterStrategy;

        public NodeFilterDelegateWrapper(ChainableNodeFilterStrategy<TNode> filterStrategy)
        {
            _filterStrategy = filterStrategy;
        }

        public bool Filter(IComparisonSource<TNode> comparisonSource, bool currentDecision)
        {
            return _filterStrategy(comparisonSource, currentDecision);
        }
    }

    class AttrFilterDelegateWrapper : IChainableAttributeFilterStrategy
    {
        private readonly ChainableAttributeFilterStrategy _filterStrategy;

        public AttrFilterDelegateWrapper(ChainableAttributeFilterStrategy filterStrategy)
        {
            _filterStrategy = filterStrategy;
        }
        public bool Filter(IAttributeComparisonSource attrComparisonSource, bool currentDecision)
        {
            return _filterStrategy(attrComparisonSource, currentDecision);
        }
    }
}
