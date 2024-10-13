namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a filter decision made by a filter.
/// </summary>
public enum FilterDecision
{
    /// <summary>
    /// Indicates the node or attribute should be part of the comparison.
    /// </summary>
    Keep = 0,
    /// <summary>
    /// Indicates the node or attribute should be excluded from the comparison.
    /// </summary>
    Exclude
}
