namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a difference found during comparison.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DiffBase<T> : IDiff where T : struct
    {
        /// <summary>
        /// Gets the control source in the comparison.
        /// </summary>
        public T Control { get; }

        /// <summary>
        /// Gets the test source in the comparison.
        /// </summary>
        public T Test { get; }

        /// <inheritdoc/>
        public DiffResult Result { get; }

        /// <inheritdoc/>
        public DiffTarget Target { get; }

        /// <summary>
        /// Instantiate the <see cref="DiffBase{T}"/>
        /// </summary>
        protected DiffBase(in T control, in T test, DiffTarget target)
        {
            Control = control;
            Test = test;
            Result = DiffResult.Different;
            Target = target;
        }
    }
}
