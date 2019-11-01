using Egil.AngleSharp.Diffing.Strategies.NodeStrategies;

namespace Egil.AngleSharp.Diffing
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

        /// <summary>
        /// Enables the basic node compare strategy during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithNodeNameComparer(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithComparer(NodeComparer.Compare, isSpecializedComparer: false);
            return builder;
        }
    }
}
