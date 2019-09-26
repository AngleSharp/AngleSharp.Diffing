using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    public interface IAttributeComparisonSource
    {
        IAttr Attribute { get; }
        IComparisonSource<IElement> ElementSource { get; }
    }
}
