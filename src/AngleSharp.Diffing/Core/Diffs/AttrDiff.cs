using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents an attribute difference
    /// </summary>
    [DebuggerDisplay("Attribute diff: Control = {Control.Path}, Test = {Test.Path}")]
    public class AttrDiff : DiffBase<AttributeComparisonSource>
    {
        /// <summary>
        /// Creates an <see cref="AttrDiff"/>.
        /// </summary>
        public AttrDiff(in AttributeComparison comparison) : base(comparison.Control, comparison.Test, DiffTarget.Attribute)
        {
        }
    }
}
