using AngleSharp.Diffing.Strategies.AttributeStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables ignoring of the special "diff"-attributes during diffing.
        /// </summary>
        public static IDiffingStrategyCollection IgnoreDiffAttributes(this IDiffingStrategyCollection builder)
        {
            builder.AddFilter(IgnoreDiffAttributesFilter.Filter, true);
            return builder;
        }

        /// <summary>
        /// Enables the name-based attribute matching strategy during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddAttributeNameMatcher(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(AttributeNameMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }

        /// <summary>
        /// Enables the basic name and value attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddAttributeComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddMatcher(PostfixedAttributeMatcher.Match, isSpecializedMatcher: true);
            builder.AddComparer(AttributeComparer.Compare, isSpecializedComparer: false);
            builder.AddComparer(IgnoreAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special class attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddClassAttributeComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(ClassAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special boolean attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddBooleanAttributeComparer(this IDiffingStrategyCollection builder, BooleanAttributeComparision booleanAttributeComparision)
        {
            builder.AddComparer(new BooleanAttributeComparer(booleanAttributeComparision).Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special style attributes comparer during diffing.
        /// </summary>
        public static IDiffingStrategyCollection AddStyleAttributeComparer(this IDiffingStrategyCollection builder)
        {
            builder.AddComparer(StyleAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }
    }
}
