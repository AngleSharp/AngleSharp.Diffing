using System;
using System.Collections.Generic;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.NodeStrategies
{
    public static class OneToOneNodeMatcher
    {
        public static IEnumerable<Comparison> Match(DiffContext context,
                                                    SourceCollection controlSources,
                                                    SourceCollection testSources)
        {
            if (controlSources is null) throw new ArgumentNullException(nameof(controlSources));
            if (testSources is null) throw new ArgumentNullException(nameof(testSources));

            using var controlsEnumerator = controlSources.GetUnmatched().GetEnumerator();
            using var testsEnumerator = testSources.GetUnmatched().GetEnumerator();
            var hasNextControl = controlsEnumerator.MoveNext();
            var hasNextTest = testsEnumerator.MoveNext();
            while (hasNextControl && hasNextTest)
            {
                yield return new Comparison(controlsEnumerator.Current, testsEnumerator.Current);
                hasNextControl = controlsEnumerator.MoveNext();
                hasNextTest = testsEnumerator.MoveNext();
            }
        }
    }
}
