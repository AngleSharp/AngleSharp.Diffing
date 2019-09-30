using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Comparers.Chainable
{
    public interface IChainableAttributeCompareStrategy
    {
        CompareResult Compare(IAttributeComparison comparison, CompareResult currentDecision);
    }
}
