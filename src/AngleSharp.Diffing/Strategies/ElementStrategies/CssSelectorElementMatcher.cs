using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;
using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Extensions;

namespace AngleSharp.Diffing.Strategies.ElementStrategies
{
    public static class CssSelectorElementMatcher
    {
        private const string DIFF_MATCH_ATTR_NAME = "diff:match";

        public static IEnumerable<Comparison> Match(IDiffContext context,
                                                    SourceCollection controlSources,
                                                    SourceCollection testSources)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (controlSources is null) throw new ArgumentNullException(nameof(controlSources));
            if (testSources is null) throw new ArgumentNullException(nameof(testSources));

            foreach (var control in controlSources.GetUnmatched())
            {
                if (control.Node.TryGetAttrValue(DIFF_MATCH_ATTR_NAME, out string cssSelector))
                {
                    if (TryGetTestNode(context, cssSelector, out var testNode))
                    {
                        var test = new ComparisonSource(testNode, ComparisonSourceType.Test);
                        yield return new Comparison(control, test);
                    }
                }
            }

            yield break;
        }

        private static bool TryGetTestNode(IDiffContext context, string cssSelector, [NotNullWhen(true)]out INode? testNode)
        {
            var searchResult = context.QueryTestRoot(cssSelector);

            testNode = searchResult.Length switch
            {
                0 => null,
                1 => searchResult[0],
                _ => throw new DiffMatchSelectorReturnedTooManyResultsException($@"The CSS selector ""{cssSelector}"" returned {searchResult.Length} matches from the test node tree. No more than one is allowed.")
            };

            return testNode is { };
        }
    }
}
