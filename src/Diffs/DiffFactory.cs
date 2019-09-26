using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing.Diffs
{
    public static class DiffFactory
    {
        public static IDiff Create(in IComparison<INode> comparison)
        {
            if (comparison is null) throw new ArgumentNullException(nameof(comparison));
            return comparison switch
            {
                IComparison<IElement> elmComp => new Diff<IElement>(in elmComp),
                IComparison<IComment> commentComp => new Diff<IComment>(in commentComp),
                IComparison<IText> textComp => new Diff<IText>(in textComp),
                _ => new Diff<INode>(in comparison)
            };
        }

        public static IDiff CreateMissing(in IComparisonSource<INode> source) => source switch
        {
            IComparisonSource<IElement> elmSrc => new MissingDiff<IElement>(in elmSrc),
            IComparisonSource<IComment> commentSrc => new MissingDiff<IComment>(in commentSrc),
            IComparisonSource<IText> textSrc => new MissingDiff<IText>(in textSrc),
            _ => new MissingDiff<INode>(source)
        };

        public static IDiff CreateUnexpected(in IComparisonSource<INode> source) => source switch
        {
            IComparisonSource<IElement> elmSrc => new UnexpectedDiff<IElement>(in elmSrc),
            IComparisonSource<IComment> commentSrc => new UnexpectedDiff<IComment>(in commentSrc),
            IComparisonSource<IText> textSrc => new UnexpectedDiff<IText>(in textSrc),
            _ => new UnexpectedDiff<INode>(source)
        };
    }
}
