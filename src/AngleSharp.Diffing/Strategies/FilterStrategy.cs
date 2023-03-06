namespace AngleSharp.Diffing.Strategies;

/// <summary>
/// Represents a filter that will inspect the <typeparamref name="TSource"/> source and decide whether or not
/// it should be included in the comparison.
///
/// If the filter does not have an opinion on whether source should be included or not, it should
/// just return the current decision provided via the <paramref name="currentDecision"/> input parameter.
/// </summary>
/// <typeparam name="TSource">The type of source to inspect, i.e. either a <see cref="AttributeComparisonSource"/> or a <see cref="ComparisonSource"/>.</typeparam>
/// <param name="source">The source node or attribute.</param>
/// <param name="currentDecision">The current decision from any previous filter or the initial decision.</param>
/// <returns>A filter decision.</returns>
public delegate FilterDecision FilterStrategy<TSource>(in TSource source, FilterDecision currentDecision);
