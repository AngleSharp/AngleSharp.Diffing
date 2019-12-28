using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a missing attribute in a test node.
    /// </summary>
    [DebuggerDisplay("Missing Attribute: Control = {Control.Path}")]
    public class MissingAttrDiff : MissingDiffBase<AttributeComparisonSource>
    {
        /// <summary>
        /// Creates a <see cref="MissingAttrDiff"/>.
        /// </summary>
        public MissingAttrDiff(in AttributeComparisonSource control) : base(control, DiffTarget.Attribute)
        {
        }
    }
}
