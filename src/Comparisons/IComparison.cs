using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    public interface IComparison<out TNode> where TNode : INode
    {
        IComparisonSource<TNode> Control { get; }

        IComparisonSource<TNode> Test { get; }
    }
}
