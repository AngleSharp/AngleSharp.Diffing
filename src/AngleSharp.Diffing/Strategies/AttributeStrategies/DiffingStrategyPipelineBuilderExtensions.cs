using AngleSharp.Diffing.Strategies.AttributeStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Enables ignoring of the special "diff"-attributes during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder IgnoreDiffAttributes(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithFilter(IgnoreDiffAttributesFilter.Filter, true);
            return builder;
        }

        /// <summary>
        /// Enables the name-based attribute matching strategy during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithAttributeNameMatcher(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithMatcher(AttributeNameMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }

        /// <summary>
        /// Enables the basic name and value attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithAttributeComparer(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithMatcher(PostfixedAttributeMatcher.Match, isSpecializedMatcher: true);
            builder.WithComparer(AttributeComparer.Compare, isSpecializedComparer: false);
            builder.WithComparer(IgnoreAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special class attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithClassAttributeComparer(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithComparer(ClassAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special boolean attribute comparer during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithBooleanAttributeComparer(this IDiffingStrategyPipelineBuilder builder, BooleanAttributeComparision booleanAttributeComparision)
        {
            builder.WithComparer(new BooleanAttributeComparer(booleanAttributeComparision).Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special style attributes comparer during diffing.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithStyleAttributeComparer(this IDiffingStrategyPipelineBuilder builder)
        {
            builder.WithComparer(StyleAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }
    }
}
