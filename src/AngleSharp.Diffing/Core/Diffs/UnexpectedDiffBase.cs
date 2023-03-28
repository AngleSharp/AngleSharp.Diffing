namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents an unexpected node or attribute in the test DOM tree.
/// </summary>
/// <param name="Test">The source of the unexpected item in the test DOM tree.</param>
/// <param name="Target"> Gets the target type in that failed the comparison.</param>
public abstract record class UnexpectedDiffBase<T>(T Test, DiffTarget Target) : IDiff where T : struct
{
    /// <inheritdoc/>
    public DiffResult Result => DiffResult.Unexpected;
}
