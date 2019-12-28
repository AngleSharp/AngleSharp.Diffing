using AngleSharp.Diffing.Core;

namespace AngleSharp.Diffing.Strategies
{
    /// <summary>
    /// Represent a comparison strategy that takes matched nodes or attributes and perform a comparison of them.
    ///
    /// If the comparer does not have an opinion on whether two matched nodes or attributes are the same, it should
    /// just return the current decision provided via the <paramref name="currentDecision"/> input parameter.
    /// </summary>
    /// <typeparam name="TComparison">The type of comparison to perform, i.e. either <see cref="AttributeComparison"/> or <see cref="Comparison"/>.</typeparam>
    /// <param name="comparison">The comparison.</param>
    /// <param name="currentDecision">The current decision from any previous comparer or the initial decision.</param>
    /// <returns>The compare result.</returns>
    public delegate CompareResult CompareStrategy<TComparison>(in TComparison comparison, CompareResult currentDecision);
}
