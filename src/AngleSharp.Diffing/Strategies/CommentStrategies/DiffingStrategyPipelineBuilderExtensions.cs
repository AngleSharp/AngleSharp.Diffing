using Egil.AngleSharp.Diffing.Strategies.CommentStrategies;

namespace Egil.AngleSharp.Diffing
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
    }
}
