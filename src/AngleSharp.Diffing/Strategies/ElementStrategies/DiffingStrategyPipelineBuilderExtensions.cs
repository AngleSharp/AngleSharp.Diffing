using AngleSharp.Diffing.Strategies;
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
            builder.AddComparer(ElementComparer.Compare, StrategyType.Generalized);
            return builder;
        }

        /// <summary>
        /// Enables the CSS-selector matcher strategy during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddCssSelectorMatcher(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(CssSelectorElementMatcher.Match, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the ignore element `diff:ignore` attribute during diffing.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDiffingStrategyCollection AddIgnoreElementSupport(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(IgnoreElementComparer.Compare, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the ignore children element `diff:ignorechildren` attribute during diffing.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDiffingStrategyCollection AddIgnoreChildrenElementSupport(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(IgnoreChildrenElementComparer.Compare, StrategyType.Specialized);
            return builder;
        }
    }
}
