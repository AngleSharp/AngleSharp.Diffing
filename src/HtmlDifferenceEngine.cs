using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;
using Egil.AngleSharp.Diffing.Diffs;

namespace Egil.AngleSharp.Diffing
{
    public class HtmlDifferenceEngine
    {
        private readonly IFilterStrategy _filterStrategy;
        private readonly IMatcherStrategy _matcherStrategy;
        private readonly ICompareStrategy _compareStrategy;

        public HtmlDifferenceEngine(IFilterStrategy filterStrategy, IMatcherStrategy matcherStrategy, ICompareStrategy compareStrategy)
        {
            _filterStrategy = filterStrategy ?? throw new ArgumentNullException(nameof(filterStrategy));
            _matcherStrategy = matcherStrategy ?? throw new ArgumentNullException(nameof(matcherStrategy)); 
            _compareStrategy = compareStrategy ?? throw new ArgumentNullException(nameof(compareStrategy)); 
        }

        public IList<IDiff> Compare(INodeList controlNodes, INodeList testNodes)
        {
            if (controlNodes is null) throw new ArgumentNullException(nameof(controlNodes));
            if (testNodes is null) throw new ArgumentNullException(nameof(testNodes));

            var controlNodeSources = controlNodes.ToComparisonSourceList(ComparisonSourceType.Control);
            var testNodeSources = testNodes.ToComparisonSourceList(ComparisonSourceType.Test);

            return CompareNodeLists(controlNodeSources, testNodeSources).ToList();
        }

        private IEnumerable<IDiff> CompareNodeLists(IEnumerable<IComparisonSource<INode>> controlNodes, IEnumerable<IComparisonSource<INode>> testNodes)
        {
            var controlsFiltered = controlNodes.Where(source => _filterStrategy.NodeFilter(in source)).ToArray();
            var testsFiltered = testNodes.Where(source => _filterStrategy.NodeFilter(in source)).ToArray();

            var comparisons = _matcherStrategy.MatchNodes(controlsFiltered, testsFiltered).ToArray();

            var diffs = CompareNodes(comparisons);

            var unmatchedDiffs = UnmatchedNodesToDiff(controlsFiltered, testsFiltered, comparisons);

            return diffs.Concat(unmatchedDiffs);
        }

        private IEnumerable<IDiff> CompareNodes(IReadOnlyList<IComparison<INode>> comparisons)
        {
            return comparisons.SelectMany(comparison =>
            {
                var nodeDiffs = CompareNode(in comparison);
                var childNodeDiffs = CompareChildNodes(in comparison);
                return nodeDiffs.Concat(childNodeDiffs);
            });
        }

        private IEnumerable<IDiff> CompareNode(in IComparison<INode> comparison)
        {
            if (comparison is IComparison<IElement> elementComparison)
            {
                return CompareElement(in elementComparison);
            }

            if (_compareStrategy.Compare(in comparison) == CompareResult.Different)
            {
                return new[] { DiffFactory.Create(in comparison) };
            }

            return Array.Empty<IDiff>();
        }

        private IEnumerable<IDiff> CompareChildNodes(in IComparison<INode> comparison)
        {
            if (!comparison.Control.Node.HasChildNodes && !comparison.Test.Node.HasChildNodes)
                return Array.Empty<IDiff>();
            
            var ctrlChildNodes = comparison.Control.Node.ChildNodes;
            var testChildNodes = comparison.Test.Node.ChildNodes;
            var ctrlPath = comparison.Control.Path;
            var testPath = comparison.Test.Path;

            return CompareNodeLists(
                ctrlChildNodes.ToComparisonSourceList(ComparisonSourceType.Control, ctrlPath),
                testChildNodes.ToComparisonSourceList(ComparisonSourceType.Test, testPath)
            );
        }

        private IEnumerable<IDiff> CompareElement(in IComparison<IElement> comparison)
        {
            var result = new List<IDiff>();

            var compareRes = _compareStrategy.Compare(in comparison);
            if (compareRes == CompareResult.Different)
            {
                result.Add(new Diff<IElement>(in comparison));
            }

            result.AddRange(CompareElementAttributes(in comparison));

            return result;
        }

        private IEnumerable<IDiff> CompareElementAttributes(in IComparison<IElement> comparison)
        {
            if (comparison.Control.Node.Attributes.Length == 0 && comparison.Test.Node.Attributes.Length == 0) return Array.Empty<IDiff>();

            var controlSrc = comparison.Control;
            var testSrc = comparison.Test;

            var controlAttrs = CreateFilteredAttributeComparisonSourceList(in controlSrc);
            var testAttrs = CreateFilteredAttributeComparisonSourceList(in testSrc);
            var attrComparisons = _matcherStrategy.MatchAttributes(controlAttrs, testAttrs).ToArray();

            var diffs = CompareAttributes(attrComparisons);
            var unmatchedDiffs = UnmatchedAttributesToDiff(controlAttrs, testAttrs, attrComparisons);

            return diffs.Concat(unmatchedDiffs);
        }

        private IEnumerable<IDiff> CompareAttributes(IEnumerable<IAttributeComparison> comparisons)
        {
            return comparisons.Where(comparison => _compareStrategy.Compare(comparison) == CompareResult.Different)
                .Select<IAttributeComparison, IDiff>(comparison => new AttrDiff(in comparison));
        }

        private IAttributeComparisonSource[] CreateFilteredAttributeComparisonSourceList(in IComparisonSource<IElement> elementComparisonSource)
        {
            var source = elementComparisonSource;
            var attributes = source.Node.Attributes;

            return attributes
                .Select<IAttr, IAttributeComparisonSource>(attr => new AttributeComparisonSource(attr, in source))
                .Where(source => _filterStrategy.AttributeFilter(in source))
                .ToArray();
        }

        private static IEnumerable<IDiff> UnmatchedNodesToDiff(IEnumerable<IComparisonSource<INode>> controls, IEnumerable<IComparisonSource<INode>> tests, IEnumerable<IComparison<INode>> comparisons)
        {
            // TODO: Since we know indexes and always increasing order of those,
            //       something more intelligent can be done here than 
            //       O(n^2) .Any(..) searches through comparisons.
            var missing = controls.Where(x => !comparisons.Any(c => c.Control == x))
                .Select(source => DiffFactory.CreateMissing(in source));
            var unexpected = tests.Where(x => !comparisons.Any(c => c.Test == x))
                .Select(source => DiffFactory.CreateUnexpected(in source));
            return missing.Concat(unexpected);
        }

        private static IEnumerable<IDiff> UnmatchedAttributesToDiff(IEnumerable<IAttributeComparisonSource> controlsAttr, IEnumerable<IAttributeComparisonSource> testsAttr, IEnumerable<IAttributeComparison> comparisons)
        {
            // TODO: Since we know indexes and always increasing order of those,
            //       something more intelligent can be done here than 
            //       O(n^2) .Any(..) searches through comparisons.
            var missing = controlsAttr.Where(x => !comparisons.Any(c => c.Control == x))
                .Select(source => DiffFactory.CreateMissing(in source));
            var unexpected = testsAttr.Where(x => !comparisons.Any(c => c.Test == x))
                .Select(source => DiffFactory.CreateUnexpected(in source));
            return missing.Concat(unexpected);
        }
    }
}
