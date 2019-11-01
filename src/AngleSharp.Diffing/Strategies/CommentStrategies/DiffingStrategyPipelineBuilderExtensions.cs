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
            builder.AddFilter(IgnoreCommentsFilter.Filter, true);
            return builder;
        }

        /// <summary>
        /// Enables the basic comment comparer, that checks if two nodes are comment nodes.
        /// </summary>
        public static IDiffingStrategyCollection AddCommentComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(CommentComparer.Compare, isSpecializedComparer: false);
            return builder;
        }
    }
}
