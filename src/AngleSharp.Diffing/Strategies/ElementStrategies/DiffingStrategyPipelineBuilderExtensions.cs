using AngleSharp.Diffing.Strategies.ElementStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables the CSS-selector matcher strategy during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithCssSelectorMatcher(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithMatcher(CssSelectorElementMatcher.Match, isSpecializedMatcher: true);
            return builder;
        }

        /// <summary>
        /// Enables the ignore element `diff:ignore` attribute during diffing.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDiffingStrategyPipelineBuilder WithIgnoreElementSupport(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithComparer(IgnoreElementComparer.Compare, isSpecializedComparer: true);
            return builder;
        }
    }
}
