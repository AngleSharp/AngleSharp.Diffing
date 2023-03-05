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
        /// Adds an element comparer that ensures element tags are closed the same way, e.g. `&lt;br&gt;` and `&lt;br /&gt;` would not be considered equal, but `&lt;br&gt;` and `&lt;br&gt;` would be.
        /// </summary>
        public static IDiffingStrategyCollection AddElementClosingComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(ElementClosingComparer.Compare, StrategyType.Specialized);
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
        /// Enables the ignore children element `diff:ignoreChildren` attribute during diffing.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDiffingStrategyCollection AddIgnoreChildrenElementSupport(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(IgnoreChildrenElementComparer.Compare, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the ignore attributes element `diff:ignoreAttributes` attribute during diffing.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDiffingStrategyCollection AddIgnoreAttributesElementSupport(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(IgnoreAttributesElementComparer.Compare, StrategyType.Specialized);
            return builder;
        }
    }
}
