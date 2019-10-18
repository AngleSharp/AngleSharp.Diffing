using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public static class AttributeNameMatcher
    {
        public static IEnumerable<AttributeComparison> Match(DiffContext context, SourceMap controlSources, SourceMap testSources)
        {
            if (controlSources is null) throw new ArgumentNullException(nameof(controlSources));
            if (testSources is null) throw new ArgumentNullException(nameof(testSources));

            foreach (var control in controlSources.GetUnmatched())
            {
                if (testSources.Contains(control.Attribute.Name) && testSources.IsUnmatched(control.Attribute.Name))
                    yield return new AttributeComparison(control, testSources[control.Attribute.Name]);
            }

            yield break;
        }
    }
}
