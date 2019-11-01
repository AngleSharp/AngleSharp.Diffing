using AngleSharp.Diffing.Strategies.TextNodeStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables the one-to-one node-matching strategy during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithTextComparer(this IDiffingStrategyPipelineBuilder builder, WhitespaceOption whitespaceOption, bool ignoreCase)
        {
            builder.WithFilter(new TextNodeFilter(whitespaceOption).Filter, isSpecializedFilter: true);
            builder.WithComparer(new TextNodeComparer(whitespaceOption, ignoreCase).Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special style-tag style sheet text comparer.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithStyleSheetComparer(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithComparer(StyleSheetTextNodeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }
    }
}
