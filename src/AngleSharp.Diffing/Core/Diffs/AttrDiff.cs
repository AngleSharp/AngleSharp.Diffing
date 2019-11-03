using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Attribute diff: Control = {Control.Path}, Test = {Test.Path}")]
    public class AttrDiff : DiffBase<AttributeComparisonSource>
    {
        public AttrDiff(in AttributeComparison comparison) : base(comparison.Control, comparison.Test, DiffTarget.Attribute)
        {
        }
    }
}
