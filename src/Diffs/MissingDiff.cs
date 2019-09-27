﻿using System;
using System.Diagnostics;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{

    [DebuggerDisplay("Diff={Target} {Result} Control={Control.Node.NodeName}[{Control.Index}]")]
    public readonly struct MissingDiff<TNode> : IDiff, IEquatable<MissingDiff<TNode>>
        where TNode : INode
    {
        public IComparisonSource<TNode> Control { get; }

        public DiffResult Result { get; }

        public DiffTarget Target { get; }

        internal MissingDiff(in IComparisonSource<TNode> control)
        {
            Control = control;
            Result = DiffResult.Missing;
            Target = control.Node.NodeType.ToDiffTarget();
        }

        public bool Equals(MissingDiff<TNode> other) => Control.Equals(other.Control) && Result == other.Result && Target == other.Target;
        public override bool Equals(object obj) => obj is MissingDiff<TNode> other && Equals(other);
        public override int GetHashCode() => (Control, Result, Target).GetHashCode();
        public static bool operator ==(MissingDiff<TNode> left, MissingDiff<TNode> right) => left.Equals(right);
        public static bool operator !=(MissingDiff<TNode> left, MissingDiff<TNode> right) => !(left == right);
    }
}
