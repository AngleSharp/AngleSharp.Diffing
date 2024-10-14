namespace AngleSharp.Diffing.Extensions;

/// <summary>
/// Represents an empty <see cref="IHtmlCollection{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public class EmptyHtmlCollection<T> : IHtmlCollection<T> where T : IElement
{
    /// <inheritdoc/>
    [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
    [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types")]
    public T this[int index] => throw new IndexOutOfRangeException();

    /// <inheritdoc/>
    [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
    [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types")]
    public T this[string id] => throw new IndexOutOfRangeException();

    /// <inheritdoc/>
    public int Length => 0;

    private EmptyHtmlCollection() { }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() { yield break; }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Gets an instance of the <see cref="EmptyHtmlCollection{T}"/>.
    /// </summary>
    public static readonly IHtmlCollection<T> Empty = new EmptyHtmlCollection<T>();
}
