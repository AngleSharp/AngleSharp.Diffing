using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;
using Egil.AngleSharp.Diffing.Diffs;

namespace Egil.AngleSharp.Diffing
{
    public enum CompareResult
    {
        Same,
        Different
    }

    public class HtmlDiffEngine
    {
        private readonly IFilterStrategy _filterStrategy;
        private readonly IMatcherStrategy _matcherStrategy;
        private readonly ICompareStrategy _compareStrategy;

        public HtmlDiffEngine(IFilterStrategy filterStrategy, IMatcherStrategy matcherStrategy, ICompareStrategy compareStrategy)
        {
            _filterStrategy = filterStrategy;
            _matcherStrategy = matcherStrategy;
            _compareStrategy = compareStrategy;
        }

        public IReadOnlyList<IDiff> Compare(INodeList controlNodes, INodeList testNodes)
        {
            if (controlNodes is null) throw new ArgumentNullException(nameof(controlNodes));
            if (testNodes is null) throw new ArgumentNullException(nameof(testNodes));

            return DoCompare(controlNodes, testNodes);
        }

        private IReadOnlyList<IDiff> DoCompare(INodeList controlNodes, INodeList testNodes, string path = "")
        {
            var result = new List<IDiff>();

            var controls = CreateFilteredComparisonSourceList(controlNodes, path, ComparisonSourceType.Control);
            var tests = CreateFilteredComparisonSourceList(testNodes, path, ComparisonSourceType.Test);

            var comparisons = _matcherStrategy.MatchNodes(controls, tests);

            var diffs = CompareNodes(comparisons);

            var unmatchedDiffs = UnmatchedNodesToDiff(controls, tests, comparisons);

            result.AddRange(diffs);
            result.AddRange(unmatchedDiffs);

            return result;
        }

        private List<IDiff> CompareNodes(IReadOnlyList<IComparison<INode>> comparisons)
        {
            var result = new List<IDiff>();
            foreach (var comparison in comparisons)
            {
                var compareRes = _compareStrategy.Compare(in comparison);
                if (compareRes == CompareResult.Different)
                {
                    result.Add(DiffFactory.Create(in comparison));
                }

                // attr compare
                // childnodes compare
            }
            return result;
        }

        private IComparisonSource<INode>[] CreateFilteredComparisonSourceList(INodeList controlNodes, string path, ComparisonSourceType sourceType)
        {
            return controlNodes.ToComparisonSourceList(path, sourceType)
                .Where(source => _filterStrategy.NodeFilter(in source))
                .ToArray();
        }

        private static IEnumerable<IDiff> UnmatchedNodesToDiff(IReadOnlyCollection<IComparisonSource<INode>> controls, IReadOnlyCollection<IComparisonSource<INode>> tests, IReadOnlyList<IComparison<INode>> comparisons)
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
    }
}
