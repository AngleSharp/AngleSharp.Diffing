namespace AngleSharp.Diffing.Extensions;

/// <summary>
/// Helper methods for <see cref="ICollection{T}"/>.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds a range of items to the target.
    /// </summary>
    public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
    {
        if (target is null)
            throw new ArgumentNullException(nameof(target));
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        foreach (var item in source)
        {
            target.Add(item);
        }
    }
}
