using AngleSharp.Diffing.Strategies.ElementStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables the basic element comparer, that checks if two nodes are element nodes and have the same name.
        /// </summary>
        public static IDiffingStrategyCollection AddElementComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(ElementComparer.Compare, isSpecializedComparer: false);
            return builder;
        }

        /// <summary>
        /// Enables the CSS-selector matcher strategy during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddCssSelectorMatcher(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(CssSelectorElementMatcher.Match, isSpecializedMatcher: true);
            return builder;
        }

        /// <summary>
        /// Enables the ignore element `diff:ignore` attribute during diffing.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDiffingStrategyCollection AddIgnoreElementSupport(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(IgnoreElementComparer.Compare, isSpecializedComparer: true);
            return builder;
        }
    }
}
