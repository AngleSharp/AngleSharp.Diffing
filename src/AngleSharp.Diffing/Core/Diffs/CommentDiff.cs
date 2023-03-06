namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a difference between two nodes.
    /// </summary>
    public class CommentDiff : NodeDiff
    {
        /// <summary>
        /// Creates a <see cref="NodeDiff"/>.
        /// </summary>
        public CommentDiff(in Comparison comparison) : base(comparison)
        {
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Comment diff: Control = {Control.Path}, Test = {Test.Path}";
        }
    }
}
