namespace AngleSharp.Diffing.Extensions;

/// <summary>
/// Represents an exception that is thrown when a part of the DOM tree is not as expected.
/// Generally not supposed to happen.
/// </summary>
[Serializable]
public sealed class UnexpectedDOMTreeStructureException : Exception
{
    /// <summary>
    /// Creates an instance of the <see cref="UnexpectedDOMTreeStructureException"/>.
    /// </summary>
    public UnexpectedDOMTreeStructureException()
        : base("The DOM tree structure was not as expected by AngleSharp.Diffing.") { }

    private UnexpectedDOMTreeStructureException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
