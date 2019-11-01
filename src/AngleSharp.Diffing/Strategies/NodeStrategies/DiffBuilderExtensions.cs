using AngleSharp.Diffing.Strategies.NodeStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffBuilderExtensions
    {
        /// <summary>
        /// Enables the one-to-one node-matching strategy during diffing.
        /// </summary>
        public static DiffBuilder WithOneToOneNodeMatcher(this DiffBuilder builder)
        {
            builder.WithMatcher(OneToOneNodeMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }

        /// <summary>
        /// Enables the forward-searching node-matcher strategy during diffing.
        /// </summary>
        public static DiffBuilder WithSearchingNodeMatcher(this DiffBuilder builder)
        {
            builder.WithMatcher(ForwardSearchingNodeMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }

        /// <summary>
        /// Enables the basic node compare strategy during diffing.
        /// </summary>
        public static DiffBuilder WithNodeNameComparer(this DiffBuilder builder)
        {
            builder.WithComparer(NodeComparer.Compare, isSpecializedComparer: false);
            return builder;
        }
    }
}
