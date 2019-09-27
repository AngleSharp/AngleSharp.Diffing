using System;
using System.Diagnostics;
using Egil.AngleSharp.Diffing.Comparisons;

namespace Egil.AngleSharp.Diffing
{
    [DebuggerDisplay("Diff={Target} {Result} Control={Control.Node.NodeName}[{Control.Index}] Test={Test.Node.NodeName}[{Test.Index}]")]
    public readonly struct AttrDiff : IDiff, IEquatable<AttrDiff>
    {
        public IAttributeComparisonSource Control { get; }

        public IAttributeComparisonSource Test { get; }

        public DiffResult Result { get; }

        public DiffTarget Target { get; }

        internal AttrDiff(in IAttributeComparison comparison)
        {
            if (comparison is null) throw new ArgumentNullException(nameof(comparison));
            Control = comparison.Control;
            Test = comparison.Test;
            Result = DiffResult.Different;
            Target = DiffTarget.Attribute;
        }

        public bool Equals(AttrDiff other) => Control.Equals(other.Control) && Test.Equals(other.Test) && Result == other.Result && Target == other.Target;
        public override bool Equals(object obj) => obj is AttrDiff other && Equals(other);
        public override int GetHashCode() => (Control, Test, Result, Target).GetHashCode();
        public static bool operator ==(AttrDiff left, AttrDiff right) => left.Equals(right);
        public static bool operator !=(AttrDiff left, AttrDiff right) => !(left == right);
    }
}
