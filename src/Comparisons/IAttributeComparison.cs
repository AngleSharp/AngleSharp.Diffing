namespace Egil.AngleSharp.Diffing.Comparisons
{
    /// <summary>
    /// Represents a comparison between two attributes, 
    /// that have been matched up for comparison.
    /// </summary>
    public interface IAttributeComparison
    {
        /// <summary>
        /// Gets the control <see cref="IAttributeComparisonSource"/>, that represents the expected attribute.
        /// </summary>
        IAttributeComparisonSource Control { get; }

        /// <summary>
        /// Gets the test <see cref="IAttributeComparisonSource"/>, which should be compared against <see cref="Control"/>.
        /// </summary>
        IAttributeComparisonSource Test { get; }
    }
}
