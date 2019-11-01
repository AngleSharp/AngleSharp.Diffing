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
        public static IDiffingStrategyCollection AddOneToOneNodeMatcher(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(OneToOneNodeMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }

        /// <summary>
        /// Enables the forward-searching node-matcher strategy during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddSearchingNodeMatcher(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(ForwardSearchingNodeMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }
    }
}
