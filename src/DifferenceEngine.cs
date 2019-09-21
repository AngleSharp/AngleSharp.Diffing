using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    public delegate bool NodeFilter(INode node);

    public class DifferenceEngine
    {
        private NodeFilter _nodeFilter = _ => true;

        public NodeFilter NodeFilter { get => _nodeFilter; set => _nodeFilter = value ?? throw new ArgumentNullException(nameof(NodeFilter)); }

        public IReadOnlyCollection<Difference> Compare(INodeList controlNodes, INodeList testNodes)
        {
            if (controlNodes is null) throw new ArgumentNullException(nameof(controlNodes));
            if (testNodes is null) throw new ArgumentNullException(nameof(testNodes));

            if (controlNodes.Length == 0 && testNodes.Length == 0)
                return Array.Empty<Difference>();

            var selectedControlNodes = controlNodes
                .WalkNodeTree()
                .Where(x => NodeFilter(x))
                .ToList();

            var selectedTestNodes = testNodes
                .WalkNodeTree()
                .Where(x => NodeFilter(x))
                .ToList();
            

            return null;
        }
    }
}
