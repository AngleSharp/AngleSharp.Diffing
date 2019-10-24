namespace Egil.AngleSharp.Diffing.Core
{
    public enum CompareResult
    {
        /// <summary>
        /// Use when the two compared nodes or attributes are the same.
        /// </summary>
        Same,
        /// <summary>
        /// Use when the two compared nodes or attributes are the different.
        /// </summary>
        Different,
        /// <summary>
        /// Use when the comparison should be skipped and any child-nodes or attributes skipped as well.
        /// </summary>
        Skip
    }

    public static class CompareResultExtensions
    {
        public static bool IsSameOrSkip(this CompareResult compareResult) => compareResult == CompareResult.Same || compareResult == CompareResult.Skip;
    }
}

