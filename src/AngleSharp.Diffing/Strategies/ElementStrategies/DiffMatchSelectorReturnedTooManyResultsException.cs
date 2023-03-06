namespace AngleSharp.Diffing.Strategies.ElementStrategies;

/// <summary>
/// Exception thrown when a the <see cref="CssSelectorElementMatcher"/> cannot find the
/// test node specified in the control node.
/// </summary>
[Serializable]
public class DiffMatchSelectorReturnedTooManyResultsException : Exception
{
    /// <summary>
    /// Creates a <see cref="DiffMatchSelectorReturnedTooManyResultsException"/>.
    /// </summary>
    public DiffMatchSelectorReturnedTooManyResultsException()
    {
    }

    /// <summary>
    /// Creates a <see cref="DiffMatchSelectorReturnedTooManyResultsException"/>.
    /// </summary>
    public DiffMatchSelectorReturnedTooManyResultsException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates a <see cref="DiffMatchSelectorReturnedTooManyResultsException"/>.
    /// </summary>
    public DiffMatchSelectorReturnedTooManyResultsException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates a <see cref="DiffMatchSelectorReturnedTooManyResultsException"/>.
    /// </summary>
    protected DiffMatchSelectorReturnedTooManyResultsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}