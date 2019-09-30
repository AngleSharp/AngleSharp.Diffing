using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Filters.Chainable
{
    public delegate bool ChainableAttributeFilterStrategy(in IAttributeComparisonSource attrComparisonSource, bool currentDecision);
}
