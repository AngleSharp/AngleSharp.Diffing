namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a missing node or attribute in the test DOM tree.
/// </summary>
/// <param name="Control">Gets the control source that has the missing item.</param>
/// <param name="Target">Gets the target type in that failed the comparison.</param>
public abstract record class MissingDiffBase<T>(T Control, DiffTarget Target) : IDiff where T : struct
{
    /// <inheritdoc/>
    public DiffResult Result => DiffResult.Missing;
}
