using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using System;
using System.Collections.Generic;

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