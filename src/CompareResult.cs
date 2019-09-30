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
}
