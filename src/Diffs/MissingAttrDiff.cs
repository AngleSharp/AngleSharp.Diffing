using System;
using System.Diagnostics;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{

    [DebuggerDisplay("Diff={Target} {Result}]")]
    public readonly struct MissingAttrDiff : IDiff, IEquatable<MissingAttrDiff>
    {
        public IAttributeComparisonSource Control { get; }

        public DiffResult Result { get; }

        public DiffTarget Target {get; }

        internal MissingAttrDiff(in IAttributeComparisonSource control)
        {
            Control = control;
            Result = DiffResult.Missing;
            Target = DiffTarget.Attribute;
        }

        public bool Equals(MissingAttrDiff other) => Control.Equals(other.Control) && Result == other.Result && Target == other.Target;
        public override bool Equals(object obj) => obj is MissingAttrDiff other && Equals(other);
        public override int GetHashCode() => (Control, Result, Target).GetHashCode();
        public static bool operator ==(MissingAttrDiff left, MissingAttrDiff right) => left.Equals(right);
        public static bool operator !=(MissingAttrDiff left, MissingAttrDiff right) => !(left == right);
    }
}
