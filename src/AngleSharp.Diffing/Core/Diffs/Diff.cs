namespace AngleSharp.Diffing.Core
{
    public class Diff : DiffBase<ComparisonSource>
    {
        public Diff(in Comparison comparison) : base(comparison.Control, comparison.Test, comparison.Control.Node.NodeType.ToDiffTarget())
        {
        }
    }
}
