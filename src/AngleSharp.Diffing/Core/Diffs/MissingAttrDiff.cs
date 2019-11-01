using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Diff={Target} {Result}]")]
    public class MissingAttrDiff : MissingDiffBase<AttributeComparisonSource>
    {
        internal MissingAttrDiff(in AttributeComparisonSource control) : base(control, DiffTarget.Attribute)
        {
        }
    }
}
