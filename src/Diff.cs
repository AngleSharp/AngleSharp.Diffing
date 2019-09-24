using System;
using System.Diagnostics;

namespace Egil.AngleSharp.Diffing
{
    [DebuggerDisplay("Diff={Type} Control={Control?.Node.NodeName}[{Control?.Index}] Test={Test?.Node.NodeName}[{Test?.Index}]")]
    public readonly struct Diff : IEquatable<Diff>
    {
        public DiffType Type { get; }

        public ComparisonSource? Control { get; }

        public ComparisonSource? Test { get; }

        public Diff(DiffType type, in ComparisonSource? control = null, in ComparisonSource? test = null)
        {
            Type = type;
            Control = control;
            Test = test;
        }

        #region Equals and Hashcode
        public bool Equals(Diff other) => Type == other.Type;
        public override int GetHashCode() => (Type).GetHashCode();
        public override bool Equals(object obj) => obj is Diff other && Equals(other);
        public static bool operator ==(Diff left, Diff right) => left.Equals(right);
        public static bool operator !=(Diff left, Diff right) => !(left == right);
        #endregion
    }

    public enum DiffType
    {
        DifferentComment,
        DifferentElementTagName,
        DifferentTextNode,
        MissingComment,
        MissingElement,
        MissingTextNode,
        UnexpectedComment,
        UnexpectedElement,
        UnexpectedTextNode,
        DifferentAttribute
    }
}
