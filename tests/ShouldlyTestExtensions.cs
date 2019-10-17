using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;

namespace Egil.AngleSharp.Diffing
{
    public static class ShouldlyTestExtensions
    {
        // The ShouldSatisfyAllConditions are here until https://github.com/shouldly/shouldly/issues/472 lands in a release of Shouldly.
        public static void ShouldSatisfyAllConditions<T>(this T actual, params Action<T>[] conditions) => ShouldSatisfyAllConditions(actual, () => null, conditions);
        public static void ShouldSatisfyAllConditions<T>(this T actual, string customMessage, params Action<T>[] conditions) => ShouldSatisfyAllConditions(actual, () => customMessage, conditions);
        public static void ShouldSatisfyAllConditions<T>(this T actual, Func<string?> customMessage, params Action<T>[] conditions)
        {
            var convertedConditions = conditions
                            .Select(a => new Action(() => a(actual)))
                            .ToArray();

            actual.ShouldSatisfyAllConditions(customMessage, convertedConditions);
        }

        public static void ShouldAllBe<T>(this IEnumerable<T> actual, Func<T, int, bool> elementPredicate)
        {
            actual.Select(elementPredicate).ShouldAllBe(x => x);
        }
    }
}
