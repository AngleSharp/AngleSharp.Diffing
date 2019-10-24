using System;
using System.Diagnostics;

namespace Egil.AngleSharp.Diffing.Core
{
    [DebuggerDisplay("Diff={Target} {Result}]")]
    public abstract class UnexpectedDiffBase<T> : IDiff where T : struct
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
    }
}
