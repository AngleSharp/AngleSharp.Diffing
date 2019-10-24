using System;
using System.Diagnostics;

namespace Egil.AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Diff={Target} {Result}")]
    public abstract class DiffBase<T> : IDiff where T : struct
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
    }
}
