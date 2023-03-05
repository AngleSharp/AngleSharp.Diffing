namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents the result of a comparison.
/// </summary>
public enum DiffResult
{
    /// <summary>
    /// Used when the sources in a comparison are different.
    /// </summary>
    Different,
    /// <summary>
    /// Used when the test source in a comparison is missing.
    /// </summary>
    Missing,
    /// <summary>
    /// Used when the test source in a comparison is was not expected.
    /// </summary>
    Unexpected
}
