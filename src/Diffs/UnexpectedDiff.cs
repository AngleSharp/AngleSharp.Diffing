using System;
using System.Diagnostics;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    [DebuggerDisplay("Diff={Target} {Result} Test={Test.Node.NodeName}[{Test.Index}]")]
    public readonly struct UnexpectedDiff<TNode> : IDiff, IEquatable<UnexpectedDiff<TNode>>
        where TNode : INode
    {
        public IComparisonSource<TNode> Test { get; }

        public DiffResult Result { get; }

        public DiffTarget Target { get; }

        internal UnexpectedDiff(in IComparisonSource<TNode> test)
        {
            Test = test;
            Result = DiffResult.Unexpected;
            Target = test.Node.NodeType.ToDiffTarget();
        }

        public bool Equals(UnexpectedDiff<TNode> other) => Test.Equals(other.Test) && Result == other.Result && Target == other.Target;
        public override bool Equals(object obj) => obj is UnexpectedDiff<TNode> other && Equals(other);
        public override int GetHashCode() => (Test, Result, Target).GetHashCode();
        public static bool operator ==(UnexpectedDiff<TNode> left, UnexpectedDiff<TNode> right) => left.Equals(right);
        public static bool operator !=(UnexpectedDiff<TNode> left, UnexpectedDiff<TNode> right) => !(left == right);
    }
}
