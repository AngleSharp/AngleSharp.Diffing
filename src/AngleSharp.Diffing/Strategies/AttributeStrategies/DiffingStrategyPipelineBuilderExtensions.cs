using AngleSharp.Diffing.Strategies;
using AngleSharp.Diffing.Strategies.AttributeStrategies;

namespace AngleSharp.Diffing
{
    /// <summary>
    /// Helper methods for registering strategies.
    /// </summary>
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables ignoring of the special "diff"-attributes during diffing.
        /// </summary>
        public static IDiffingStrategyCollection IgnoreDiffAttributes(this IDiffingStrategyCollection builder)
        {
            builder.AddFilter(IgnoreDiffAttributesFilter.Filter, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the name-based attribute matching strategy during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddAttributeNameMatcher(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(AttributeNameMatcher.Match, StrategyType.Generalized);
            return builder;
        }

        /// <summary>
        /// Enables the basic name and value attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddAttributeComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(PostfixedAttributeMatcher.Match, StrategyType.Specialized);
            builder.AddComparer(AttributeComparer.Compare, StrategyType.Generalized);
            builder.AddComparer(IgnoreAttributeComparer.Compare, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the special class attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddClassAttributeComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(ClassAttributeComparer.Compare, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the special boolean attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddBooleanAttributeComparer(this IDiffingStrategyCollection builder, BooleanAttributeComparision booleanAttributeComparision)
        {
            builder.AddComparer(new BooleanAttributeComparer(booleanAttributeComparision).Compare, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the special style attributes comparer during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddStyleAttributeComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(StyleAttributeComparer.Compare, StrategyType.Specialized);
            return builder;
        }

        /// <summary>
        /// Enables the special style attributes comparer during diffing and orderers the styles alphabetically before comparing them.
        /// </summary>
        public static IDiffingStrategyCollection AddOrderedStyleAttributeComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(OrderedStyleAttributeComparer.Compare, StrategyType.Specialized);
            return builder;
        }
    }
}
