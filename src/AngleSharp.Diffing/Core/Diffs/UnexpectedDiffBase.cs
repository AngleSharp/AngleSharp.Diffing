namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents an unexpected node or attribute in the test DOM tree.
/// </summary>
public abstract class UnexpectedDiffBase<T> : IDiff where T : struct
{
    /// <summary>
    /// The source of the unexpected item in the test DOM tree.
    /// </summary>
    public T Test { get; }

    /// <inheritdoc/>
    public DiffResult Result { get; }

    /// <inheritdoc/>
    public DiffTarget Target { get; }

    /// <summary>
    /// Creates a <see cref="UnexpectedDiffBase{T}"/>.
    /// </summary>
    protected UnexpectedDiffBase(in T test, DiffTarget target)
    {
        Test = test;
        Result = DiffResult.Unexpected;
        Target = target;
    }
}
