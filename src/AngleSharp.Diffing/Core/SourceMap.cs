namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a map of <see cref="AttributeComparisonSource"/> sources.
/// </summary>
[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
public class SourceMap : IEnumerable<AttributeComparisonSource>
{
    private readonly HashSet<string> _matched = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, AttributeComparisonSource> _sources = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the type of the sources in the collection.
    /// </summary>
    public ComparisonSourceType SourceType { get; }

    /// <summary>
    /// Gets the number of items in the map.
    /// </summary>
    public int Count => _sources.Count;

    /// <summary>
    /// Gets the <see cref="AttributeComparisonSource"/> with the specified <paramref name="name"/>.
    /// </summary>
    public AttributeComparisonSource this[string name]
    {
        get
        {
            return _sources[name];
        }
    }

    /// <summary>
    /// Creates a <see cref="SourceMap"/>.
    /// </summary>
    public SourceMap(in ComparisonSource elementSource)
    {
        if (elementSource.Node is IElement element)
        {
            SourceType = elementSource.SourceType;
            foreach (var attr in element.Attributes)
            {
                _sources.Add(attr.Name, new AttributeComparisonSource(attr.Name, elementSource));
            }
        }
        else
        {
            throw new ArgumentException("An attribute source map cannot be created unless a element comparison source is provided.", nameof(elementSource));
        }
    }

    /// <summary>
    /// Checks whether the map contains a source with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Contains(string name) => _sources.ContainsKey(name);

    /// <summary>
    /// Checks whether the source with the name <paramref name="name"/> has been matched.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsUnmatched(string name) => !_matched.Contains(name);

    /// <summary>
    /// Gets all the unmatched sources in the map.
    /// </summary>
    public IEnumerable<AttributeComparisonSource> GetUnmatched()
    {
        foreach (var source in _sources.Values)
        {
            if (!_matched.Contains(source.Attribute.Name))
                yield return source;
        }
    }

    /// <inheritdoc/>
    public IEnumerator<AttributeComparisonSource> GetEnumerator() => _sources.Values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Removes a source from the map.
    /// </summary>
    public void Remove(SourceMapRemovePredicate predicate)
    {
        var removeQueue = new Queue<string>(Count);
        foreach (var source in _sources.Values)
        {
            if (predicate(source) == FilterDecision.Exclude)
                removeQueue.Enqueue(source.Attribute.Name);
        }
        foreach (var name in removeQueue)
        {
            _sources.Remove(name);
        }
    }

    /// <summary>
    /// Mark a source as matched.
    /// </summary>
    public void MarkAsMatched(in AttributeComparisonSource source)
    {
        _matched.Add(source.Attribute.Name);
    }
}