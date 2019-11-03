using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Missing {Target}: Control = {Control.Path}")]
    public class MissingNodeDiff : MissingDiffBase<ComparisonSource>
    {
        public MissingNodeDiff(in ComparisonSource control) : base(control, control.Node.NodeType.ToDiffTarget())
        {
        }
    }
}
