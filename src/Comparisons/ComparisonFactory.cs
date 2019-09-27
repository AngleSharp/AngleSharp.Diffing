using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    public static class ComparisonFactory
    {
        public static IComparison<INode> Create(in IComparisonSource<INode> control, in IComparisonSource<INode> test)
        {
            if (control is null) throw new System.ArgumentNullException(nameof(control));
            if (test is null) throw new System.ArgumentNullException(nameof(test));

            return control switch
            {
                IComparisonSource<IElement> elmCtrl when (test is IComparisonSource<IElement> elmTest)
                    => new Comparison<IElement>(in elmCtrl, in elmTest),
                IComparisonSource<IComment> cmtCtrl when (test is IComparisonSource<IComment> cmtTest)
                    => new Comparison<IComment>(in cmtCtrl, in cmtTest),
                IComparisonSource<IText> textCtrl when (test is IComparisonSource<IText> textTest)
                    => new Comparison<IText>(in textCtrl, in textTest),
                _ => new Comparison<INode>(in control, in test)
            };
        }
    }
}
