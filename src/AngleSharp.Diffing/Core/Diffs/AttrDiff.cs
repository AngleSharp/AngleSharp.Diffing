namespace Egil.AngleSharp.Diffing.Core
{
    public class AttrDiff : DiffBase<AttributeComparisonSource>
    {
        public AttrDiff(in AttributeComparison comparison) : base(comparison.Control, comparison.Test, DiffTarget.Attribute)
        {
        }
    }
}
