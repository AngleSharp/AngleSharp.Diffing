using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using AngleSharp.Dom;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represent a comparison between two nodes.
    /// </summary>
    [DebuggerDisplay("Control: {Control.Path} | Test: {Test.Path}")]
    public readonly struct Comparison : IEquatable<Comparison>
    {
        /// <summary>
        /// Gets the control source in the comparison.
        /// </summary>
        public ComparisonSource Control { get; }

        /// <summary>
        /// Gets the test source in the comparison
        /// </summary>
        public ComparisonSource Test { get; }

        /// <summary>
        /// Gets whether the control and test nodes are of the same type and has the same name.
        /// </summary>
        public bool AreNodeTypesEqual => Control.Node.NodeType == Test.Node.NodeType && Control.Node.NodeName == Test.Node.NodeName;

        /// <summary>
        /// Creates a new comparison.
        /// </summary>
        /// <param name="control">The control source in the comparison.</param>
        /// <param name="test">The tes tsource in the comparison.</param>
        public Comparison(in ComparisonSource control, in ComparisonSource test)
        {
            Control = control;
            Test = test;
        }

        /// <summary>
        /// Try to get the control and test node as the <typeparamref name="TNode"/> type.
        /// Returns true if both test and control node is of type <typeparamref name="TNode"/>, false otherwise.
        /// </summary>
        /// <typeparam name="TNode">THe type of node to try to retrieve.</typeparam>
        /// <returns>Returns true if both test and control node is of type <typeparamref name="TNode"/>, false otherwise.</returns>
        public bool TryGetNodesAsType<TNode>([NotNullWhen(true)]out TNode? controlNode, [NotNullWhen(true)]out TNode? testNode) where TNode : class, INode
        {
            if (Control.Node is TNode ctrl && Test.Node is TNode test)
            {
                controlNode = ctrl;
                testNode = test;
                return true;
            }
            else
            {
                controlNode = default;
                testNode = default;
                return false;
            }
        }

        #region Equals and HashCode
        /// <inheritdoc/>
        public bool Equals(Comparison other) => Control.Equals(other.Control) && Test.Equals(other.Test);
        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Comparison other && Equals(other);
        /// <inheritdoc/>
        public override int GetHashCode() => (Control, Test).GetHashCode();
        /// <inheritdoc/>
        public static bool operator ==(Comparison left, Comparison right) => left.Equals(right);
        /// <inheritdoc/>
        public static bool operator !=(Comparison left, Comparison right) => !left.Equals(right);
        #endregion
    }
}
