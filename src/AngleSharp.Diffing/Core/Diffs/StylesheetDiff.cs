namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a difference between the applied style of two nodes
    /// </summary>
    public class StylesheetDiff : NodeDiff
    {
        /// <summary>
        /// Creates a <see cref="StylesheetDiff"/>.
        /// </summary>
        public StylesheetDiff(in Comparison comparison) : base(comparison)
        {
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Stylesheet diff: Control = {Control.Path}, Test = {Test.Path}";
        }
    }
}
