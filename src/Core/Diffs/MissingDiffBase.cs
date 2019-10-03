using System;

namespace Egil.AngleSharp.Diffing.Core
{
    public abstract class MissingDiffBase<T> : IDiff, IEquatable<MissingDiffBase<T>> where T : struct
    {
        public T Control { get; }

        public DiffResult Result { get; }

        public DiffTarget Target { get; }

        protected MissingDiffBase(in T control, DiffTarget target)
        {
            Control = control;
            Result = DiffResult.Missing;
            Target = target;
        }

        public bool Equals(MissingDiffBase<T> other) => !(other is null) && Control.Equals(other.Control) && Result == other.Result && Target == other.Target;
        public override bool Equals(object obj) => obj is MissingDiffBase<T> other && Equals(other);
        public override int GetHashCode() => (Control, Result, Target).GetHashCode();
        public static bool operator ==(MissingDiffBase<T> left, MissingDiffBase<T> right)
        {
            if (left is null && right is null)
                return true;
            if (!(left is null))
                return left.Equals(right);
            return false;
        }

        public static bool operator !=(MissingDiffBase<T> left, MissingDiffBase<T> right) => !(left == right);
    }
}
