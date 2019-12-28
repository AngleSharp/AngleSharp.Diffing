using System.Diagnostics;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents an unexpected node in the test DOM tree.
    /// </summary>
    [DebuggerDisplay("Unexpected {Target}: Test = {Test.Path}")]
    public class UnexpectedNodeDiff : UnexpectedDiffBase<ComparisonSource>
    {
        /// <summary>
        /// Creates a <see cref="UnexpectedNodeDiff"/>.
        /// </summary>
        public UnexpectedNodeDiff(in ComparisonSource test) : base(test, test.Node.NodeType.ToDiffTarget())
        {
        }
    }
}
