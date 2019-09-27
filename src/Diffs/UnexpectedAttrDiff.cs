using System;
using System.Diagnostics;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    [DebuggerDisplay("Diff={Target} {Result}]")]
    public readonly struct UnexpectedAttrDiff : IDiff, IEquatable<UnexpectedAttrDiff>
    {
        public IAttributeComparisonSource Test { get; }

        public DiffResult Result { get; }

        public DiffTarget Target { get; }

        internal UnexpectedAttrDiff(in IAttributeComparisonSource test)
        {
            Test = test;
            Result = DiffResult.Unexpected;
            Target = DiffTarget.Attribute;
        }

        public bool Equals(UnexpectedAttrDiff other) => Test.Equals(other.Test) && Result == other.Result && Target == other.Target;
        public override bool Equals(object obj) => obj is UnexpectedAttrDiff other && Equals(other);
        public override int GetHashCode() => (Test, Result, Target).GetHashCode();
        public static bool operator ==(UnexpectedAttrDiff left, UnexpectedAttrDiff right) => left.Equals(right);
        public static bool operator !=(UnexpectedAttrDiff left, UnexpectedAttrDiff right) => !(left == right);
    }
}
