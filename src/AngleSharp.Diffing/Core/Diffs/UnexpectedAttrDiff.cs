using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Unexpected Attribute: Test = {Test.Path}")]
    public class UnexpectedAttrDiff : UnexpectedDiffBase<AttributeComparisonSource>
    {
        public UnexpectedAttrDiff(in AttributeComparisonSource test) : base(test, DiffTarget.Attribute)
        {
        }
    }
}
