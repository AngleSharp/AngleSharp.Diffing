using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.NodeStrategies
{
    public static class OneToOneNodeMatcher
    {
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "context param is needed to have the proper signature")]
        public static IEnumerable<Comparison> Match(DiffContext context,
                                                    SourceCollection controlSources,
                                                    SourceCollection testSources)
        {
            if (controlSources is null) throw new ArgumentNullException(nameof(controlSources));
            if (testSources is null) throw new ArgumentNullException(nameof(testSources));

            var controlsEnumerator = controlSources.GetUnmatched().GetEnumerator();
            var testEnumerator = testSources.GetUnmatched().GetEnumerator();
            var hasNextControl = controlsEnumerator.MoveNext();
            var hasNextTest = testEnumerator.MoveNext();
            while (hasNextControl && hasNextTest)
            {
                yield return new Comparison(controlsEnumerator.Current, testEnumerator.Current);
                hasNextControl = controlsEnumerator.MoveNext();
                hasNextTest = testEnumerator.MoveNext();
            }
        }
    }
}
