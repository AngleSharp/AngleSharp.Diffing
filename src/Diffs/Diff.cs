using System;
using System.Diagnostics;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{

    [DebuggerDisplay("Diff={Target} {Result} Control={Control.Node.NodeName}[{Control.Index}] Test={Test.Node.NodeName}[{Test.Index}]")]
    public readonly struct Diff<TNode> : IDiff, IEquatable<Diff<TNode>>
        where TNode : INode
    {
        public IComparisonSource<TNode> Control { get; }

        public IComparisonSource<TNode> Test { get; }

        public DiffResult Result { get; }

        public NodeType Target => Control.Node.NodeType;

        internal Diff(in IComparison<TNode> comparison)
        {
            if (comparison is null) throw new ArgumentNullException(nameof(comparison));
            Control = comparison.Control;
            Test = comparison.Test;
            Result = DiffResult.Different;
        }

        public bool Equals(Diff<TNode> other) => Control.Equals(other.Control) && Test.Equals(other.Test) && Result == other.Result;
        public override bool Equals(object obj) => obj is Diff<TNode> other && Equals(other);
        public override int GetHashCode() => (Control, Test, Result).GetHashCode();
        public static bool operator ==(Diff<TNode> left, Diff<TNode> right) => left.Equals(right);
        public static bool operator !=(Diff<TNode> left, Diff<TNode> right) => !(left == right);
    }
}
