using System;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

namespace AngleSharp.Diffing
{
    public static class ShouldlyTestExtensions
    {        
        public static void ShouldAllBe<T>(this IEnumerable<T> actual, Func<T, int, bool> elementPredicate)
        {
            actual.Select(elementPredicate).ShouldAllBe(x => x);
        }
    }
}
