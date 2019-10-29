using Egil.AngleSharp.Diffing.Strategies.AttributeStrategies;

namespace Egil.AngleSharp.Diffing
{
    public static partial class DiffBuilderExtensions
    {
        /// <summary>
        /// Enables ignoring of the special "diff"-attributes during diffing.
        /// </summary>
        public static DiffBuilder IgnoreDiffAttributes(this DiffBuilder builder)
        {
            builder.WithFilter(IgnoreDiffAttributesFilter.Filter, true);
            return builder;
        }

        /// <summary>
        /// Enables the name-based attribute matching strategy during diffing.
        /// </summary>
        public static DiffBuilder WithAttributeNameMatcher(this DiffBuilder builder)
        {
            builder.WithMatcher(AttributeNameMatcher.Match, isSpecializedMatcher: false);
            return builder;
        }

        /// <summary>
        /// Enables the basic name and value attribute comparer during diffing.
        /// </summary>
        public static DiffBuilder WithAttributeComparer(this DiffBuilder builder)
        {
            builder.WithMatcher(PostfixedAttributeMatcher.Match, isSpecializedMatcher: true);
            builder.WithComparer(AttributeComparer.Compare, isSpecializedComparer: false);
            builder.WithComparer(IgnoreAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special class attribute comparer during diffing.
        /// </summary>
        public static DiffBuilder WithClassAttributeComparer(this DiffBuilder builder)
        {
            builder.WithComparer(ClassAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special boolean attribute comparer during diffing.
        /// </summary>
        public static DiffBuilder WithBooleanAttributeComparer(this DiffBuilder builder, BooleanAttributeComparision booleanAttributeComparision)
        {
            builder.WithComparer(new BooleanAttributeComparer(booleanAttributeComparision).Compare, isSpecializedComparer: true);
            return builder;
        }

        /// <summary>
        /// Enables the special style attributes comparer during diffing.
        /// </summary>
        public static DiffBuilder WithStyleAttributeComparer(this DiffBuilder builder)
        {
            builder.WithComparer(StyleAttributeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }
    }
}
