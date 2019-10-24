using System;

namespace Egil.AngleSharp.Diffing.Core
{
    public abstract class MissingDiffBase<T> : IDiff where T : struct
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
    }
}
