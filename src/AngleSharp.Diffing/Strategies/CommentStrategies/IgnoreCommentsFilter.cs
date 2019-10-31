using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.CommentStrategies
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
