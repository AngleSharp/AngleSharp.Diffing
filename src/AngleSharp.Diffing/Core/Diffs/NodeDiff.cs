using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    [DebuggerDisplay("{Target} diff: Control = {Control.Path}, Test = {Test.Path}")]
    public class NodeDiff : DiffBase<ComparisonSource>
    {
        public NodeDiff(in Comparison comparison) : base(comparison.Control, comparison.Test, comparison.Control.Node.NodeType.ToDiffTarget())
        {
        }
    }
}
