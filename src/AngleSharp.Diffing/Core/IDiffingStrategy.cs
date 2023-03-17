namespace AngleSharp.Diffing.Core;

/// <summary>
/// This interface needs to be implemented if you want to provide your own diffing strategies
/// used by the <see cref="HtmlDifferenceEngine"/> during diffing.
/// </summary>
public interface IDiffingStrategy
{
    /// <summary>
    /// Decides whether a node should be part of the comparison.
    /// </summary>
    /// <param name="comparisonSource">A comparison source for the node</param>
    /// <returns>true if the node should be part of the comparison, false if the node should be filtered out.</returns>
    FilterDecision Filter(in ComparisonSource comparisonSource);

    /// <summary>
    /// Decides whether an attribute should be part of the comparison.
    /// </summary>
    /// <param name="attributeComparisonSource">A attribute comparison source for the attribute</param>
    /// <returns>true if the attribute should be part of the comparison, false if the attribute should be filtered out.</returns>
    FilterDecision Filter(in AttributeComparisonSource attributeComparisonSource);

    /// <summary>
    /// Matches up the control nodes with test nodes in the two input lists. The matched control and test nodes will be compared to each other.
    /// Any unmatched control or test nodes will be reported as either <see cref="DiffResult.Missing"/> or <see cref="DiffResult.Unexpected"/>,
    /// depending on whether they are control or test nodes.
    /// </summary>
    /// <returns></returns>
    IEnumerable<Comparison> Match(IDiffContext context, SourceCollection controlSources, SourceCollection testSources);

    /// <summary>
    /// Matches up the control attributes with test attributes in the two input lists. The matched control and test attributes will be compared to each other.
    /// Any unmatched control or test attributes will be reported as either <see cref="DiffResult.Missing"/> or <see cref="DiffResult.Unexpected"/>,
    /// depending on whether they are control or test attributes.
    /// </summary>
    /// <returns></returns>
    IEnumerable<AttributeComparison> Match(IDiffContext context, SourceMap controlAttrSources, SourceMap testAttrSources);

    /// <summary>
    /// Compares the control and test nodes in the <see cref="Comparison"/> object and
    /// tells the caller whether they are to be considered <see cref="CompareResult.Different"/> or
    /// <see cref="CompareResult.Same"/>.
    /// </summary>
    /// <returns><see cref="CompareResult.Same"/> if the comparison is successful, <see cref="CompareResult.Different"/> otherwise.</returns>
    CompareResult Compare(in Comparison comparison);

    /// <summary>
    /// Compares the control and test attributes in the <see cref="AttributeComparison"/> object and
    /// tells the caller whether they are to be considered <see cref="CompareResult.Different"/> or
    /// <see cref="CompareResult.Same"/>.
    /// </summary>
    /// <returns><see cref="CompareResult.Same"/> if the comparison is successful, <see cref="CompareResult.Different"/> otherwise.</returns>
    CompareResult Compare(in AttributeComparison comparison);
}
