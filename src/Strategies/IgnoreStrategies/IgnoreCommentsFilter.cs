using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.AngleSharp.Diffing.Strategies.IgnoreStrategies
{
    public static class IgnoreCommentsFilter
    {
        public static FilterDecision Filter(in ComparisonSource source, FilterDecision currentDecision)
        {
            if (currentDecision.IsExclude()) return currentDecision;

            if (source.Node.NodeType == NodeType.Comment) return FilterDecision.Exclude;

            return currentDecision;
        }
    }
}
