namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents an difference where the attributes are in different order.
    /// </summary>
    public class DifferentAttrOrderDiff : AttrDiff
    {
        /// <summary>
        /// Creates an <see cref="AttrNameDiff"/>.
        /// </summary>
        public DifferentAttrOrderDiff(in AttributeComparison comparison) : base(comparison)
        {
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Attribute order diff: Control = {Control.Path}, Test = {Test.Path}";
        }
    }
}
