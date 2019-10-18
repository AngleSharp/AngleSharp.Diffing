using System;
using System.Collections.Generic;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public static class IgnoreAttributeMatcher
    {
        private const string IGNORE_ATTR_POSTFIX = ":ignore";
        private static readonly int IGNORE_ATTR_POSTFIX_LENGTH = IGNORE_ATTR_POSTFIX.Length;

        public static IEnumerable<AttributeComparison> Match(DiffContext context, SourceMap controlSources, SourceMap testSources)
        {
            if (controlSources is null) throw new ArgumentNullException(nameof(controlSources));
            if (testSources is null) throw new ArgumentNullException(nameof(testSources));

            foreach (var control in controlSources.GetUnmatched())
            {
                if (!control.Attribute.Name.EndsWith(IGNORE_ATTR_POSTFIX, StringComparison.Ordinal)) continue;

                var attrName = control.Attribute.Name.Substring(0, control.Attribute.Name.Length - IGNORE_ATTR_POSTFIX_LENGTH);

                if (testSources.Contains(attrName) && testSources.IsUnmatched(attrName))
                    yield return new AttributeComparison(control, testSources[attrName]);
            }

            yield break;
        }
    }
}
