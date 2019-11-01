using AngleSharp.Diffing.Strategies.CommentStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables ignoring HTML comments during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder IgnoreComments(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithFilter(IgnoreCommentsFilter.Filter, true);
            return builder;
        }

        /// <summary>
        /// Enables the basic comment comparer, that checks if two nodes are comment nodes.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithCommentComparer(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithComparer(CommentComparer.Compare, isSpecializedComparer: false);
            return builder;
        }
    }
}
