using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;

namespace AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public static class PostfixedAttributeMatcher
    {
        private readonly static string[] POSTFIXES = new string[] {
            ":ignore",
            ":ignorecase",
            ":regex"
        };

        public static IEnumerable<AttributeComparison> Match(IDiffContext context, SourceMap controlSources, SourceMap testSources)
        {
            if (controlSources is null) throw new ArgumentNullException(nameof(controlSources));
            if (testSources is null) throw new ArgumentNullException(nameof(testSources));

            foreach (var control in controlSources.GetUnmatched())
            {
                var ctrlName = control.Attribute.Name;
                if (!NameHasPostfixSeparator(ctrlName)) continue;

                ctrlName = RemovePostfixFromName(ctrlName);

                if (testSources.Contains(ctrlName) && testSources.IsUnmatched(ctrlName))
                    yield return new AttributeComparison(control, testSources[ctrlName]);
            }

            yield break;
        }

        private static bool NameHasPostfixSeparator(string ctrlName)
        {
            return ctrlName.Contains(':');
        }

        private static string RemovePostfixFromName(string ctrlName)
        {
            for (int index = 0; index < POSTFIXES.Length; index++)
            {
                if (ctrlName.EndsWith(POSTFIXES[index], StringComparison.Ordinal))
                {
                    ctrlName = ctrlName.Substring(0, ctrlName.Length - POSTFIXES[index].Length);
                    index = NameHasPostfixSeparator(ctrlName) ? -1 : POSTFIXES.Length;
                }
            }

            return ctrlName;
        }
    }
}
