using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies
{
    public static class IgnoreDiffAttributesFilter
    {
        private const string DiffAttributePrefix = "diff:";

        public static bool Filter(in AttributeComparisonSource source, bool currentDecision)
        {
            if (!currentDecision) return currentDecision;

            if (source.Attribute.Name.StartsWith(DiffAttributePrefix, StringComparison.OrdinalIgnoreCase))
                return false;

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
