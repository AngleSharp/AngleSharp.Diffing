using System;
using AngleSharp.Dom;
using System.Diagnostics.CodeAnalysis;

namespace Egil.AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represent a comparison between two nodes.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public readonly struct Comparison : IEquatable<Comparison>
    {
        public ComparisonSource Control { get; }

        public ComparisonSource Test { get; }

        public Comparison(in ComparisonSource control, in ComparisonSource test)
        {
            Control = control;
            Test = test;
        }

        public bool AreNodeTypesEqual() => Control.Node.NodeType == Test.Node.NodeType && Control.Node.NodeName == Test.Node.NodeName;

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
        public bool Equals(Comparison other) => Control == other.Control && Test == other.Test;
        public override bool Equals(object obj) => obj is Comparison other && Equals(other);
        public override int GetHashCode() => (Control, Test).GetHashCode();
        public static bool operator ==(Comparison left, Comparison right) => left.Equals(right);
        public static bool operator !=(Comparison left, Comparison right) => !(left == right);
        #endregion
    }
}
