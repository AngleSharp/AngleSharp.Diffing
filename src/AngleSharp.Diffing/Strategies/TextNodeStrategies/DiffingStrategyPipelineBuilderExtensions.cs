using AngleSharp.Diffing.Strategies;
using AngleSharp.Diffing.Strategies.TextNodeStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables the one-to-one node-matching strategy during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddTextComparer(this IDiffingStrategyCollection builder, WhitespaceOption whitespaceOption, bool ignoreCase)
        {
            builder.AddFilter(new TextNodeFilter(whitespaceOption).Filter, StrategyType.Specialized);
            builder.AddComparer(new TextNodeComparer(whitespaceOption, ignoreCase).Compare, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the special style-tag style sheet text comparer.
        /// </summary>
        public static IDiffingStrategyCollection AddStyleSheetComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(StyleSheetTextNodeComparer.Compare, StrategyType.Specialized);
            return builder;
        }
    }
}
