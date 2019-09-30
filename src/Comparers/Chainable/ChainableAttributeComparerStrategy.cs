using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Comparers.Chainable
{
    public delegate CompareResult ChainableAttributeComparerStrategy(in IAttributeComparison comparison, CompareResult currentDecision);
}
