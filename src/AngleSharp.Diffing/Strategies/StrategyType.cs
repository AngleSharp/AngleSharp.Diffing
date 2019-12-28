namespace AngleSharp.Diffing.Strategies
{
    /// <summary>
    /// Represents a type of strategy in a <see cref="IDiffingStrategyCollection"/> type.
    /// </summary>
    public enum StrategyType
    {
        /// <summary>
        /// Indicates the strategy is a general strategy, that targets all nodes or attributes.
        /// </summary>
        Generalized,
        /// <summary>
        /// Indicates that the strategy is a specialized strategy, that only targets specific or select nodes or attributes.
        /// </summary>
        Specialized
    }
}