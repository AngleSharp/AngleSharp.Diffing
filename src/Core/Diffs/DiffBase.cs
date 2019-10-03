using System;
using System.Diagnostics;

namespace Egil.AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Diff={Target} {Result}")]
    public abstract class DiffBase<T> : IDiff, IEquatable<DiffBase<T>>
        where T : struct
    {
        public T Control { get; }

        public T Test { get; }

        public DiffResult Result { get; }

        public DiffTarget Target { get; }

        protected DiffBase(in T control, in T test, DiffTarget target)
        {
            Control = control;
            Test = test;
            Result = DiffResult.Different;
            Target = target;
        }

        public bool Equals(DiffBase<T> other) => !(other is null) && Control.Equals(other.Control) && Test.Equals(other.Test) && Result == other.Result && Target == other.Target;
        public override bool Equals(object obj) => obj is DiffBase<T> other && Equals(other);
        public override int GetHashCode() => (Control, Test, Result, Target).GetHashCode();
        public static bool operator ==(DiffBase<T> left, DiffBase<T> right)
        {
            if (left is null && right is null)
                return true;
            if (!(left is null))
                return left.Equals(right);
            return false;
        }
        public static bool operator !=(DiffBase<T> left, DiffBase<T> right) => !(left == right);
    }
}
