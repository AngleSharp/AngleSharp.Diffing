using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.IgnoreStrategies
{
    public static class IgnoreDiffAttributesFilter
    {
        private const string DiffAttributePrefix = "diff:";

        public static FilterDecision Filter(in AttributeComparisonSource source, FilterDecision currentDecision)
        {
            if (currentDecision.IsExclude()) return currentDecision;

            if (source.Attribute.Name.StartsWith(DiffAttributePrefix, StringComparison.OrdinalIgnoreCase))
                return FilterDecision.Exclude;

            return currentDecision;
        }

        public static DiffBuilder IgnoreDiffAttributes(this DiffBuilder builder)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));
            builder.WithFilter(Filter);
            return builder;
        }
    }
}
