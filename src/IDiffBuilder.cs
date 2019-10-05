using System;
using System.Collections.Generic;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing
{
    public interface IDiffBuilder
    {
        string Control { get; set; }
        string Test { get; set; }

        IList<IDiff> Build();
        DiffBuilder WithTest(string test);
        DiffBuilder WithFilter(Func<ComparisonSource, bool> nodeFilter);
    }
}