using System;

using AngleSharp.Diffing.Core;

namespace AngleSharp.Diffing.Strategies.AttributeStrategies
{
    /// <summary>
    /// Represents the ignore diff attribute filter.
    /// </summary>
    public static class IgnoreDiffAttributesFilter
    {
        private const string DiffAttributePrefix = "diff:";

        /// <summary>
        /// The ignore diff attribute filter.
        /// </summary>
        public static FilterDecision Filter(in AttributeComparisonSource source, FilterDecision currentDecision)
        {
            if (currentDecision.IsExclude())
                return currentDecision;

            if (source.Attribute.Name.StartsWith(DiffAttributePrefix, StringComparison.OrdinalIgnoreCase))
                return FilterDecision.Exclude;

            return currentDecision;
        }
    }
}
