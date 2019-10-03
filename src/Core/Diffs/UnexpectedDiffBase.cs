using System;
using System.Diagnostics;

namespace Egil.AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Diff={Target} {Result}]")]
    public abstract class UnexpectedDiffBase<T> : IDiff, IEquatable<UnexpectedDiffBase<T>> where T : struct
    {
        public T Test { get; }

        public DiffResult Result { get; }

        public DiffTarget Target { get; }

        protected UnexpectedDiffBase(in T test, DiffTarget target)
        {
            Test = test;
            Result = DiffResult.Unexpected;
            Target = target;
        }

        public bool Equals(UnexpectedDiffBase<T> other) => !(other is null) && Test.Equals(other.Test) && Result == other.Result && Target == other.Target;
        public override bool Equals(object obj) => obj is UnexpectedDiffBase<T> other && Equals(other);
        public override int GetHashCode() => (Test, Result, Target).GetHashCode();
        public static bool operator ==(UnexpectedDiffBase<T> left, UnexpectedDiffBase<T> right)
        {
            if (left is null && right is null)
                return true;
            if (!(left is null))
                return left.Equals(right);
            return false;
        }

        public static bool operator !=(UnexpectedDiffBase<T> left, UnexpectedDiffBase<T> right) => !(left == right);
    }
}
