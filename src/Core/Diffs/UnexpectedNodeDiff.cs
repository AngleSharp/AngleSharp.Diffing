using System;
using System.Diagnostics;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Diff={Target} {Result} Test={Test.Node.NodeName}[{Test.Index}]")]
    public class UnexpectedNodeDiff : UnexpectedDiffBase<ComparisonSource>
    {
        public UnexpectedNodeDiff(in ComparisonSource test) : base(test, test.Node.NodeType.ToDiffTarget())
        {
        }
    }
}
