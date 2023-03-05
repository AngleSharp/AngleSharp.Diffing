namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a missing node or attribute in the test DOM tree.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MissingDiffBase<T> : IDiff where T : struct
{
    /// <summary>
    /// Gets the control source that has the missing item.
    /// </summary>
    public T Control { get; }

    /// <inheritdoc/>
    public DiffResult Result { get; }

    /// <inheritdoc/>
    public DiffTarget Target { get; }

    /// <summary>
    /// Create a <see cref="MissingDiffBase{T}"/>
    /// </summary>
    protected MissingDiffBase(in T control, DiffTarget target)
    {
        Control = control;
        Result = DiffResult.Missing;
        Target = target;
    }
}
