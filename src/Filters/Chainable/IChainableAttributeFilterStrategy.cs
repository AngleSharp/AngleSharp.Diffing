using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Filters
{
    public interface IChainableAttributeFilterStrategy
    {
        bool Filter(IAttributeComparisonSource attrComparisonSource, bool currentDecision);
    }
}
