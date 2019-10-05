using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Extensions;

namespace Egil.AngleSharp.Diffing.Core
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

            var controlSources = controlNodes.ToSourceCollection(ComparisonSourceType.Control);
            var testSources = testNodes.ToSourceCollection(ComparisonSourceType.Test);

            var context = CreateDiffContext(controlNodes, testNodes);

            var diffs = CompareNodeLists(context, controlSources, testSources);
            var unmatchedDiffs = context.GetDiffsFromUnmatched();

            return diffs.Concat(unmatchedDiffs).ToList();
        }

        private static DiffContext CreateDiffContext(INodeList controlNodes, INodeList testNodes)
        {
            IElement? controlRoot = null;
            IElement? testRoot = null;

            if (controlNodes.Length > 0 && controlNodes[0].GetRoot() is IElement r1) { controlRoot = r1; }
            if (testNodes.Length > 0 && testNodes[0].GetRoot() is IElement r2) { testRoot = r2; }

            return new DiffContext(controlRoot, testRoot);
        }

        private IEnumerable<IDiff> CompareNodeLists(DiffContext context, SourceCollection controlSources, SourceCollection testSources)
        {
            ApplyNodeFilter(controlSources);
            ApplyNodeFilter(testSources);
            var comparisons = MatchNodes(context, controlSources, testSources);
            var diffs = CompareNodes(context, comparisons);

            return diffs;
        }

        private void ApplyNodeFilter(SourceCollection sources)
        {
            sources.Remove(_filterStrategy.NodeFilter);
        }

        private IEnumerable<Comparison> MatchNodes(DiffContext context, SourceCollection controls, SourceCollection tests)
        {
            foreach (var comparison in _matcherStrategy.MatchNodes(context, controls, tests))
            {
                MarkSelectedSourcesAsMatched(comparison);
                yield return comparison;
            }

            UpdateUnmatchedTracking();

            yield break;

            void MarkSelectedSourcesAsMatched(in Comparison comparison)
            {
                controls.MarkAsMatched(comparison.Control);
                tests.MarkAsMatched(comparison.Test);
                context.MissingSources.Remove(comparison.Control);
                context.UnexpectedSources.Remove(comparison.Test);
            }

            void UpdateUnmatchedTracking()
            {
                context.MissingSources.AddRange(controls.GetUnmatched());
                context.UnexpectedSources.AddRange(tests.GetUnmatched());
            }
        }

        private IEnumerable<IDiff> CompareNodes(DiffContext context, IEnumerable<Comparison> comparisons)
        {
            return comparisons.SelectMany(comparison => CompareNode(context, comparison));
        }

        private IEnumerable<IDiff> CompareNode(DiffContext context, in Comparison comparison)
        {
            if (comparison.Control.Node is IElement)
            {
                return CompareElement(context, comparison);
            }

            var compareRes = _compareStrategy.Compare(comparison);
            if (compareRes == CompareResult.Different || compareRes == CompareResult.DifferentAndBreak)
            {
                IDiff diff = new Diff(comparison);
                return new[] { diff };
            }

            return Array.Empty<IDiff>();
        }

        private IEnumerable<IDiff> CompareElement(DiffContext context, in Comparison comparison)
        {
            var result = new List<IDiff>();

            var compareRes = _compareStrategy.Compare(comparison);
            if (compareRes == CompareResult.Different || compareRes == CompareResult.DifferentAndBreak)
            {
                result.Add(new Diff(comparison));
            }

            if (compareRes == CompareResult.Same || compareRes == CompareResult.Different)
            {
                result.AddRange(CompareElementAttributes(context, comparison));
                result.AddRange(CompareChildNodes(context, comparison));
            }

            return result;
        }

        private IEnumerable<IDiff> CompareElementAttributes(DiffContext context, in Comparison comparison)
        {
            if (!comparison.Control.Node.HasAttributes() && !comparison.Test.Node.HasAttributes()) return Array.Empty<IDiff>();

            var controlAttrs = new SourceMap(comparison.Control);
            var testAttrs = new SourceMap(comparison.Test);

            ApplyFilterAttributes(controlAttrs);
            ApplyFilterAttributes(testAttrs);

            var attrComparisons = MatchAttributes(context, controlAttrs, testAttrs);

            return CompareAttributes(attrComparisons);
        }

        private void ApplyFilterAttributes(SourceMap controlAttrs)
        {
            controlAttrs.Remove(_filterStrategy.AttributeFilter);
        }

        private IEnumerable<AttributeComparison> MatchAttributes(DiffContext context, SourceMap controls, SourceMap tests)
        {
            foreach (var comparison in _matcherStrategy.MatchAttributes(context, controls, tests))
            {
                MarkSelectedSourcesAsMatched(comparison);
                yield return comparison;
            }

            UpdateUnmatchedTracking();

            yield break;

            void MarkSelectedSourcesAsMatched(in AttributeComparison comparison)
            {
                controls.MarkAsMatched(comparison.Control);
                tests.MarkAsMatched(comparison.Test);
                context.MissingAttributeSources.Remove(comparison.Control);
                context.UnexpectedAttributeSources.Remove(comparison.Test);
            }

            void UpdateUnmatchedTracking()
            {
                context.MissingAttributeSources.AddRange(controls.GetUnmatched());
                context.UnexpectedAttributeSources.AddRange(tests.GetUnmatched());
            }
        }

        private IEnumerable<IDiff> CompareChildNodes(DiffContext context, in Comparison comparison)
        {
            if (!comparison.Control.Node.HasChildNodes && !comparison.Test.Node.HasChildNodes)
                return Array.Empty<IDiff>();

            var ctrlChildNodes = comparison.Control.Node.ChildNodes;
            var testChildNodes = comparison.Test.Node.ChildNodes;
            var ctrlPath = comparison.Control.Path;
            var testPath = comparison.Test.Path;

            return CompareNodeLists(
                context,
                ctrlChildNodes.ToSourceCollection(ComparisonSourceType.Control, ctrlPath),
                testChildNodes.ToSourceCollection(ComparisonSourceType.Test, testPath)
            );
        }

        private IEnumerable<IDiff> CompareAttributes(IEnumerable<AttributeComparison> comparisons)
        {
            foreach (var comparison in comparisons)
            {
                var compareRes = _compareStrategy.Compare(comparison);
                if (compareRes == CompareResult.Different || compareRes == CompareResult.DifferentAndBreak)
                    yield return new AttrDiff(comparison);
            }
        }
    }
}
