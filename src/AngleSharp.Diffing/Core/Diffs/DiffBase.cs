namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference found during comparison.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Control">Gets the control source in the comparison.</param>
/// <param name="Test">Gets the test source in the comparison.</param>
/// <param name="Target"> Gets the target type in that failed the comparison.</param>
public abstract record DiffBase<T>(T Control, T Test, DiffTarget Target) : IDiff where T : struct
{
    /// <inheritdoc/>
    public DiffResult Result => DiffResult.Different;
}
