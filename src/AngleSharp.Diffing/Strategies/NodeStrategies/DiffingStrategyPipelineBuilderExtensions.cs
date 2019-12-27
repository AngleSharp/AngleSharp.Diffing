using AngleSharp.Diffing.Strategies;
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
            builder.AddMatcher(OneToOneNodeMatcher.Match, StrategyType.Generalized);
            return builder;
        }

        /// <summary>
        /// Enables the forward-searching node-matcher strategy during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddSearchingNodeMatcher(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(ForwardSearchingNodeMatcher.Match, StrategyType.Generalized);
            return builder;
        }
    }
}
