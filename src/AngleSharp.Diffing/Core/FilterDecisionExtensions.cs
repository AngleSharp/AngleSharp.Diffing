namespace AngleSharp.Diffing.Core;

/// <summary>
/// Helper methods for <see cref="FilterDecision"/>.
/// </summary>
public static class FilterDecisionExtensions
{
    /// <summary>
    /// Gets whether the <see cref="FilterDecision"/> is <see cref="FilterDecision.Exclude"/>.
    /// </summary>
    public static bool IsExclude(this FilterDecision decision) => decision == FilterDecision.Exclude;

    /// <summary>
    /// Gets whether the <see cref="FilterDecision"/> is <see cref="FilterDecision.Keep"/>.
    /// </summary>
    public static bool IsKeep(this FilterDecision decision) => decision == FilterDecision.Keep;
}
