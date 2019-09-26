using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    public interface IMatcherStrategy
    {        
        IReadOnlyList<IComparison<INode>> MatchNodes(IReadOnlyList<IComparisonSource<INode>> controlNodes, IReadOnlyList<IComparisonSource<INode>> testNodes);
        IReadOnlyList<IAttributeComparison> MatchAttributes(IComparison<IElement> elementComparison);
    }
}
