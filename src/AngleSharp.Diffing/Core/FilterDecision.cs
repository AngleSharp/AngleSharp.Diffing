namespace AngleSharp.Diffing.Core
{
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
}
