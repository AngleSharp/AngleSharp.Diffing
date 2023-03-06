namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a difference between the types of two nodes.
    /// </summary>
    public class NodeTypeDiff : NodeDiff
    {
        /// <summary>
        /// Creates a <see cref="NodeTypeDiff"/>.
        /// </summary>
        public NodeTypeDiff(in Comparison comparison) : base(comparison)
        {
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Node type diff: Control = {Control.Path}, Test = {Test.Path}";
        }
    }
}
