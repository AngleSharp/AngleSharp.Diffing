using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    public interface IFilterStrategy
    {
        bool NodeFilter(in IComparisonSource<INode> comparisonSource);
        bool AttributeFilter(in IAttributeComparisonSource attributeComparisonSource);
    }
}
