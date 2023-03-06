namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents an attribute difference with different values
    /// </summary>
    public class AttrValueDiff : AttrDiff
    {
        /// <summary>
        /// The kind of the value.
        /// </summary>
        public AttributeValueKind ValueKind { get; }

        /// <summary>
        /// Creates an <see cref="AttrValueDiff"/>.
        /// </summary>
        public AttrValueDiff(in AttributeComparison comparison, AttributeValueKind valueKind) : base(comparison)
        {
            ValueKind = valueKind;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Attribute value diff: Control = {Control.Path}, Test = {Test.Path}, Kind = {ValueKind}";
        }
    }

    /// <summary>
    /// Defines the type of attribute.
    /// </summary>
    public enum AttributeValueKind
    {
        /// <summary>
        /// The value is not further specified.
        /// </summary>
        Unspecified,
        /// <summary>
        /// The value is a boolean.
        /// </summary>
        Boolean,
        /// <summary>
        /// The value is a class.
        /// </summary>
        Class,
        /// <summary>
        /// The value is a style name.
        /// </summary>
        Style
    }
}
