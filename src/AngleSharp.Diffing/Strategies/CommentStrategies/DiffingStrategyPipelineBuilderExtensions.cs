using AngleSharp.Diffing.Strategies;
using AngleSharp.Diffing.Strategies.CommentStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables ignoring HTML comments during diffing.
        /// </summary>
        public static IDiffingStrategyCollection IgnoreComments(this IDiffingStrategyCollection builder)
        {
            builder.AddFilter(IgnoreCommentsFilter.Filter, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the basic comment comparer, that checks if two nodes are comment nodes. It does not check the
        /// content of the comment.
        /// </summary>
        public static IDiffingStrategyCollection AddCommentComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(CommentComparer.Compare, StrategyType.Generalized);
            return builder;
        }
    }
}
