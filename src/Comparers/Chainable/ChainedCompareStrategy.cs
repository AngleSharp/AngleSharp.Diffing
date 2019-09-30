using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Comparers.Chainable
{
    public class ChainedCompareStrategy : ICompareStrategy
    {
        private readonly List<IChainableNodeCompareStrategy<IElement>> _elementComparers = new List<IChainableNodeCompareStrategy<IElement>>();
        private readonly List<IChainableNodeCompareStrategy<IComment>> _commentComparers = new List<IChainableNodeCompareStrategy<IComment>>();
        private readonly List<IChainableNodeCompareStrategy<IText>> _textComparers = new List<IChainableNodeCompareStrategy<IText>>();
        private readonly List<IChainableNodeCompareStrategy<INode>> _nodeComparers = new List<IChainableNodeCompareStrategy<INode>>();
        private readonly List<IChainableAttributeCompareStrategy> _attrComparers = new List<IChainableAttributeCompareStrategy>();

        public CompareResult Compare<TNode>(in IComparison<TNode> nodeComparison) where TNode : INode
        {
            return nodeComparison switch
            {
                IComparison<IElement> comparison => ApplyNodeComparers(in comparison, _elementComparers),
                IComparison<IComment> comparison => ApplyNodeComparers(in comparison, _commentComparers),
                IComparison<IText> comparison => ApplyNodeComparers(in comparison, _textComparers),
                IComparison<INode> comparison => ApplyNodeComparers(in comparison, _nodeComparers),
                _ => throw new InvalidOperationException("Unknown comparison type")
            };
        }

        private CompareResult ApplyNodeComparers<TNode>(in IComparison<TNode> comparison, List<IChainableNodeCompareStrategy<TNode>> comparers) where TNode : INode
        {
            var result = CompareResult.Same;

            foreach (var comparer in comparers)
            {
                result = comparer.Compare(comparison, result);
            }

            return result;
        }

        public CompareResult Compare(in IAttributeComparison comparison)
        {
            var result = CompareResult.Same;

            foreach (var comparer in _attrComparers)
            {
                result = comparer.Compare(comparison, result);
            }

            return result;
        }

        public void AddComparer<TNode>(ChainableNodeComparerStrategy<TNode> comparer)
            where TNode : INode
        {
            switch (comparer)
            {
                case ChainableNodeComparerStrategy<IElement> filter:
                    AddComparer(new NodeComparerDelegateWrapper<IElement>(filter));
                    break;
                case ChainableNodeComparerStrategy<IComment> filter:
                    AddComparer(new NodeComparerDelegateWrapper<IComment>(filter));
                    break;
                case ChainableNodeComparerStrategy<IText> filer:
                    AddComparer(new NodeComparerDelegateWrapper<IText>(filer));
                    break;
                case ChainableNodeComparerStrategy<INode> filer:
                    AddComparer(new NodeComparerDelegateWrapper<INode>(filer));
                    break;
            }
        }

        public void AddComparer(ChainableAttributeComparerStrategy comparer) => AddComparer(new AttrComparerDelegateWrapper(comparer));

        public void AddComparer(IChainableNodeCompareStrategy<IElement> compareStrategy)
        {
            _elementComparers.Add(compareStrategy);
        }

        public void AddComparer(IChainableNodeCompareStrategy<IComment> compareStrategy)
        {
            _commentComparers.Add(compareStrategy);
        }

        public void AddComparer(IChainableNodeCompareStrategy<IText> compareStrategy)
        {
            _textComparers.Add(compareStrategy);
        }

        public void AddComparer(IChainableNodeCompareStrategy<INode> compareStrategy)
        {
            _elementComparers.Add(compareStrategy);
            _commentComparers.Add(compareStrategy);
            _textComparers.Add(compareStrategy);
            _nodeComparers.Add(compareStrategy);
        }

        public void AddComparer(IChainableAttributeCompareStrategy compareStrategy)
        {
            _attrComparers.Add(compareStrategy);
        }
    }

    class NodeComparerDelegateWrapper<TNode> : IChainableNodeCompareStrategy<TNode> where TNode : INode
    {
        private readonly ChainableNodeComparerStrategy<TNode> _strategy;

        public NodeComparerDelegateWrapper(ChainableNodeComparerStrategy<TNode> strategy)
        {
            _strategy = strategy;
        }

        public CompareResult Compare(IComparison<TNode> comparison, CompareResult currentDecision)
        {
            return _strategy(comparison, currentDecision);
        }
    }

    class AttrComparerDelegateWrapper : IChainableAttributeCompareStrategy
    {
        private readonly ChainableAttributeComparerStrategy _strategy;

        public AttrComparerDelegateWrapper(ChainableAttributeComparerStrategy strategy)
        {
            _strategy = strategy;
        }

        public CompareResult Compare(IAttributeComparison comparison, CompareResult currentDecision)
        {
            return _strategy(comparison, currentDecision);
        }
    }
}
