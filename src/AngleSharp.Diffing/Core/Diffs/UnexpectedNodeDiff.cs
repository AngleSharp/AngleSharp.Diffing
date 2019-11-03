using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Unexpected {Target}: Test = {Test.Path}")]
    public class UnexpectedNodeDiff : UnexpectedDiffBase<ComparisonSource>
    {
        public UnexpectedNodeDiff(in ComparisonSource test) : base(test, test.Node.NodeType.ToDiffTarget())
        {
        }
    }
}
