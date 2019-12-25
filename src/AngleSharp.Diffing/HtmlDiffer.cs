using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace AngleSharp.Diffing
{
    public class HtmlDiffer
    {
        private readonly IDiffingStrategy _diffingStrategy;

        public HtmlDiffer(IDiffingStrategy diffingStrategy)
        {
            _diffingStrategy = diffingStrategy ?? throw new ArgumentNullException(nameof(diffingStrategy));
        }

        public IEnumerable<IDiff> Compare(INode controlNode, IEnumerable<INode> testNodes)
        {
            if (controlNode is null) throw new ArgumentNullException(nameof(controlNode));
            if (testNodes is null) throw new ArgumentNullException(nameof(testNodes));

            return Compare(new[] { controlNode }, testNodes);
        }

        public IEnumerable<IDiff> Compare(IEnumerable<INode> controlNodes, INode testNode)
        {
            if (controlNodes is null) throw new ArgumentNullException(nameof(controlNodes));
            if (testNode is null) throw new ArgumentNullException(nameof(testNode));

            return Compare(controlNodes, new[] { testNode });
        }

        public IEnumerable<IDiff> Compare(INode controlNode, INode testNode)
        {
            if (controlNode is null) throw new ArgumentNullException(nameof(controlNode));
            if (testNode is null) throw new ArgumentNullException(nameof(testNode));

            return Compare(new[] { controlNode }, new[] { testNode });
        }

        public IEnumerable<IDiff> Compare(IEnumerable<INode> controlNodes, IEnumerable<INode> testNodes)
        {
            if (controlNodes is null) throw new ArgumentNullException(nameof(controlNodes));
            if (testNodes is null) throw new ArgumentNullException(nameof(testNodes));

            var controlSources = controlNodes.ToSourceCollection(ComparisonSourceType.Control);
            var testSources = testNodes.ToSourceCollection(ComparisonSourceType.Test);

            return new HtmlDifferenceEngine(_diffingStrategy, controlSources, testSources).Compare();
        }
    }
}
