using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Missing Attribute: Control = {Control.Path}")]
    public class MissingAttrDiff : MissingDiffBase<AttributeComparisonSource>
    {
        internal MissingAttrDiff(in AttributeComparisonSource control) : base(control, DiffTarget.Attribute)
        {
        }
    }
}
