namespace AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    /// <summary>
    /// Represents a whitespace diffing option.
    /// </summary>
    public enum WhitespaceOption
    {
        /// <summary>
        /// Does not change or filter out any whitespace in text nodes the control and test HTML.
        /// </summary>
        Preserve = 0,
        /// <summary>
        /// Using this option filters out all text nodes that only consist of whitespace characters.
        /// </summary>
        RemoveWhitespaceNodes,
        /// <summary>
        /// Using this option will trim all text nodes and replace two or more whitespace characters with a single space character.
        /// This option implicitly includes the <see cref="RemoveWhitespaceNodes" /> option.
        /// </summary>
        Normalize
    }
}
