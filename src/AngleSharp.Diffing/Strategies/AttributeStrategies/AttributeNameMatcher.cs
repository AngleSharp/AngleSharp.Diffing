using System;
using System.Collections.Generic;

using AngleSharp.Diffing.Core;

namespace AngleSharp.Diffing.Strategies.AttributeStrategies
{
    /// <summary>
    /// Attribute name matcher strategy
    /// </summary>
    public static class AttributeNameMatcher
    {
        /// <summary>
        /// Attribute name matcher strategy.
        /// </summary>
        public static IEnumerable<AttributeComparison> Match(IDiffContext context, SourceMap controlSources, SourceMap testSources)
        {
            if (controlSources is null)
                throw new ArgumentNullException(nameof(controlSources));
            if (testSources is null)
                throw new ArgumentNullException(nameof(testSources));

            foreach (var control in controlSources.GetUnmatched())
            {
                if (testSources.Contains(control.Attribute.Name) && testSources.IsUnmatched(control.Attribute.Name))
                    yield return new AttributeComparison(control, testSources[control.Attribute.Name]);
            }

            yield break;
        }
    }
}
