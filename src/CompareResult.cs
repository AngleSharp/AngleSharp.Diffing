namespace Egil.AngleSharp.Diffing
{
    public enum CompareResult
    {
        /// <summary>
        /// Indicates the two compared nodes or attributes are the same.
        /// </summary>
        Same,
        /// <summary>
        /// Indicates the two compared nodes or attributes are the same AND no further comparison should happen on any child nodes or attributes.
        /// </summary>
        SameAndBreak,
        /// <summary>
        /// Indicates the two compared nodes or attributes are the different.
        /// </summary>
        Different,
        /// <summary>
        /// Indicates the two compared nodes or attributes are the different AND no further comparison should happen on any child nodes or attributes.
        /// </summary>
        DifferentAndBreak
    }

    public static class CompareResultExtensions
    {
        public static bool IsSame(this CompareResult compareResult) => compareResult == CompareResult.Same;
        public static bool IsSameAndBreak(this CompareResult compareResult) => compareResult == CompareResult.SameAndBreak;
        public static bool IsDifferent(this CompareResult compareResult) => compareResult == CompareResult.Different;
        public static bool IsDifferentAndBreak(this CompareResult compareResult) => compareResult == CompareResult.DifferentAndBreak;
    }
}

