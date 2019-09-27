using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Comparisons
{
    /// <summary>
    /// Represents a single comparison between a control node and a test node.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public interface IComparison<out TNode> where TNode : INode
    {
        /// <summary>
        /// Gets the Control node source, which should be used as the baseline to compare the <see cref="Test"/> node source with.
        /// </summary>
        IComparisonSource<TNode> Control { get; }

        /// <summary>
        /// Gets the Test node source, which should be compare to the <see cref="Control"/> node source.
        /// </summary>
        IComparisonSource<TNode> Test { get; }
    }
}
