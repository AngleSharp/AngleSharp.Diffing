using AngleSharp.Diffing.Strategies.CommentStrategies;
using AngleSharp.Diffing.Strategies.ElementStrategies;
using AngleSharp.Diffing.Strategies.NodeStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables the one-to-one node-matching strategy during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithOneToOneNodeMatcher(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithMatcher(OneToOneNodeMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }

        /// <summary>
        /// Enables the forward-searching node-matcher strategy during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithSearchingNodeMatcher(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithMatcher(ForwardSearchingNodeMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }
    }
}
