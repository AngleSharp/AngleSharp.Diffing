using System;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a result of a comparison.
    /// </summary>
    [Flags]
    public enum CompareResult
    {
        /// <summary>
        /// Use when the two compared nodes or attributes are the same.
        /// </summary>
        Same = 1,
        /// <summary>
        /// Use when the two compared nodes or attributes are the different.
        /// </summary>
        Different = 2,
        /// <summary>
        /// Use when the comparison should be skipped and any child-nodes or attributes skipped as well.
        /// </summary>
        Skip = 4,
        /// <summary>
        /// Use when the comparison should skip any child-nodes.
        /// </summary>
        SkipChildren = 8,
    }

    /// <summary>
    /// Helper methods for <see cref="CompareResult"/>
    /// </summary>
    public static class CompareResultExtensions
    {
        /// <summary>
        /// Checks if a <see cref="CompareResult"/> is either a <see cref="CompareResult.Same"/> or <see cref="CompareResult.Skip"/>.
        /// </summary>
        /// <param name="compareResult">The compare result</param>
        public static bool IsSameOrSkip(this CompareResult compareResult) => compareResult == CompareResult.Same || compareResult == CompareResult.Skip;
    }
}

