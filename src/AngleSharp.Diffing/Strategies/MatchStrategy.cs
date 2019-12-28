using System.Collections.Generic;

using AngleSharp.Diffing.Core;

namespace AngleSharp.Diffing.Strategies
{
    /// <summary>
    /// Represents a matching strategy used to match two sets of collection of elements or two sets of attributes together into
    /// <see cref="Comparison"/> or <see cref="AttributeComparison"/> matches.
    ///
    /// The matcher should only match up the sources it knows how to match up. The rest will be handled
    /// by other matchers or be left as unmatched and reported as either missing or unexpected by the
    /// diffing engine.
    /// </summary>
    /// <typeparam name="TSources">The source type, i.e. <see cref="ComparisonSource"/> or <see cref="AttributeComparisonSource"/>.</typeparam>
    /// <typeparam name="TComparison">The resulting match type, i.e. <see cref="Comparison"/> or <see cref="AttributeComparison"/>.</typeparam>
    /// <param name="context">A diffing context that provides methods for querying the control and test DOM tree.</param>
    /// <param name="controlSources">The control sources.</param>
    /// <param name="testSources">The test sources.</param>
    /// <returns>Any matches found. Can be lazy populated enumerable, e.g. via <c>yield return</c>.</returns>
    public delegate IEnumerable<TComparison> MatchStrategy<in TSources, out TComparison>(IDiffContext context, TSources controlSources, TSources testSources);
}
