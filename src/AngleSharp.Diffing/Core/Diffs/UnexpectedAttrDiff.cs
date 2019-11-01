using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Diff={Target} {Result}]")]
    public class UnexpectedAttrDiff : UnexpectedDiffBase<AttributeComparisonSource>
    {
        public UnexpectedAttrDiff(in AttributeComparisonSource test) : base(test, DiffTarget.Attribute)
        {
        }
    }
}
