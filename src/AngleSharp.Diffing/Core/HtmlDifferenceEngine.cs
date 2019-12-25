using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Diffing.Extensions;

namespace AngleSharp.Diffing.Core
{
    // TODO: Create a class that holds all working data for a diff, and all the diffing logic itself. A mix between DiffContext and HtmlDifferenceEngine.
    public class HtmlDifferenceEngine
    {
        private readonly IDiffingStrategy _diffingStrategy;

        public HtmlDifferenceEngine(IDiffingStrategy diffingStrategy)
        {
            _diffingStrategy = diffingStrategy ?? throw new ArgumentNullException(nameof(diffingStrategy));
        }

        public IEnumerable<IDiff> Compare(INode controlNode, INode testNode)
        {
            if (controlNode is null) throw new ArgumentNullException(nameof(controlNode));
            if (testNode is null) throw new ArgumentNullException(nameof(testNode));

            return Compare(new[] { controlNode }, new[] { testNode });
        }

        public IEnumerable<IDiff> Compare(IEnumerable<INode> controlNodes, IEnumerable<INode> testNodes)
        {
            if (controlNodes is null) throw new ArgumentNullException(nameof(controlNodes));
            if (testNodes is null) throw new ArgumentNullException(nameof(testNodes));

            var controlSources = controlNodes.ToSourceCollection(ComparisonSourceType.Control);
            var testSources = testNodes.ToSourceCollection(ComparisonSourceType.Test);

            return Compare(controlSources, testSources);
        }

        public IEnumerable<IDiff> Compare(SourceCollection controlSources, SourceCollection testSources)
        {
            if (controlSources is null) throw new ArgumentNullException(nameof(controlSources));
            if (testSources is null) throw new ArgumentNullException(nameof(testSources));

            var context = CreateDiffContext(controlSources, testSources);

            var diffs = CompareNodeLists(context, controlSources, testSources);
            var unmatchedDiffs = context.GetDiffsFromUnmatched();

            return diffs.Concat(unmatchedDiffs);
        }

        private static DiffContext CreateDiffContext(SourceCollection controlNodes, SourceCollection testNodes)
        {
            IElement? controlRoot = null;
            IElement? testRoot = null;

            if (controlNodes.Count > 0 && controlNodes.First().Node.GetRoot() is IElement r1) { controlRoot = r1; }
            if (testNodes.Count > 0 && testNodes.First().Node.GetRoot() is IElement r2) { testRoot = r2; }

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
            sources.Remove(_diffingStrategy.Filter);
        }

        private IEnumerable<Comparison> MatchNodes(DiffContext context, SourceCollection controls, SourceCollection tests)
        {
            foreach (var comparison in _diffingStrategy.Match(context, controls, tests))
            {
                UpdateMatchedTracking(comparison);
                yield return comparison;
            }

            UpdateUnmatchedTracking();

            yield break;

            void UpdateMatchedTracking(in Comparison comparison)
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

            var compareRes = _diffingStrategy.Compare(comparison);
            if (compareRes == CompareResult.Different)
            {
                IDiff diff = new NodeDiff(comparison);
                return new[] { diff };
            }

            return Array.Empty<IDiff>();
        }

        private IEnumerable<IDiff> CompareElement(DiffContext context, in Comparison comparison)
        {
            var result = new List<IDiff>();

            var compareRes = _diffingStrategy.Compare(comparison);
            if (compareRes == CompareResult.Different)
            {
                result.Add(new NodeDiff(comparison));
            }

            if (compareRes != CompareResult.Skip)
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
            controlAttrs.Remove(_diffingStrategy.Filter);
        }

        private IEnumerable<AttributeComparison> MatchAttributes(DiffContext context, SourceMap controls, SourceMap tests)
        {
            foreach (var comparison in _diffingStrategy.Match(context, controls, tests))
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
                var compareRes = _diffingStrategy.Compare(comparison);
                if (compareRes == CompareResult.Different)
                    yield return new AttrDiff(comparison);
            }
        }
    }
}
