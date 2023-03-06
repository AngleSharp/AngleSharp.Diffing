namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents an attribute difference with different names
    /// </summary>
    public class AttrNameDiff : AttrDiff
    {
        /// <summary>
        /// Creates an <see cref="AttrNameDiff"/>.
        /// </summary>
        public AttrNameDiff(in AttributeComparison comparison) : base(comparison)
        {
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Attribute name diff: Control = {Control.Path}, Test = {Test.Path}";
        }
    }
}
