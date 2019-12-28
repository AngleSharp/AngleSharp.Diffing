using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AngleSharp.Diffing.Core
{
    /// <summary>
    /// Represents a collection of <see cref="ComparisonSource"/> sources in a comparison.
    /// </summary>
    public class SourceCollection : IEnumerable<ComparisonSource>
    {
        private const int SOURCE_REMOVED = -1;
        private const int SOURCE_UNMATCHED = 0;
        private const int SOURCE_MATCHED = 1;

        private readonly int[] _status;
        private IReadOnlyList<ComparisonSource> _sources;

        /// <summary>
        /// Gets the type of the sources in the collection.
        /// </summary>
        public ComparisonSourceType SourceType { get; }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the source at the specified index.
        /// </summary>
        public ComparisonSource this[int index]
        {
            get
            {
                if (_status[_sources[index].Index] == SOURCE_REMOVED)
                    throw new InvalidOperationException("The source at the specified index has been removed.");
                return _sources[index];
            }
        }

        /// <summary>
        /// Creates a new <see cref="SourceCollection"/>.
        /// </summary>
        public SourceCollection(ComparisonSourceType sourceType, IEnumerable<ComparisonSource> sources) : this(sourceType, sources.ToArray()) { }

        /// <summary>
        /// Creates a new <see cref="SourceCollection"/>.
        /// </summary>
        public SourceCollection(ComparisonSourceType sourceType, IReadOnlyList<ComparisonSource> sources)
        {
            SourceType = sourceType;
            _sources = sources;
            _status = new int[_sources.Count];
            Count = _sources.Count;
        }

        /// <summary>
        /// Gets all the unmatched sources in the collection, starting from the specified index (default: 0).
        /// </summary>
        public IEnumerable<ComparisonSource> GetUnmatched(int startIndex = 0)
        {
            for (int i = startIndex; i < _sources.Count; i++)
            {
                if (_status[_sources[i].Index] == SOURCE_UNMATCHED)
                {
                    yield return _sources[i];
                }
            }
            yield break;
        }

        /// <inheritdoc/>
        public IEnumerator<ComparisonSource> GetEnumerator()
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                var source = _sources[i];
                EnsureSourceAtExpectedIndex(i, source);

                if (_status[source.Index] != SOURCE_REMOVED)
                {
                    yield return source;
                }
            }
            yield break;
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets all the sources originally in the collection, even those removed and those marked as matched.
        /// </summary>
        public IEnumerable<ComparisonSource> GetAllSources()
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                var source = _sources[i];
                EnsureSourceAtExpectedIndex(i, source);

                yield return source;
            }
        }

        /// <summary>
        /// Mark a source as matched. After it has been marked, it will not be returned by <see cref="GetUnmatched(int)"/>.
        /// </summary>
        public void MarkAsMatched(in ComparisonSource source)
        {
            if (_status[source.Index] == SOURCE_REMOVED)
                throw new InvalidOperationException("A removed source cannot be marked as matched. The source is not supposed to be part of the comparison.");

            _status[source.Index] = SOURCE_MATCHED;
        }

        /// <summary>
        /// Apply a filter predicate to the collection. All matched sources will not be returned
        /// by <see cref="GetUnmatched(int)"/> or by <see cref="GetEnumerator"/>.
        /// </summary>
        /// <param name="predicate"></param>
        public void Remove(SourceCollectionRemovePredicate predicate)
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                var source = _sources[i];
                if (predicate(source) == FilterDecision.Exclude)
                {
                    _status[source.Index] = SOURCE_REMOVED;
                    Count--;
                }
            }
        }

        private static void EnsureSourceAtExpectedIndex(int expectedIndex, ComparisonSource source)
        {
            if (source.Index != expectedIndex)
                throw new InvalidOperationException($"The source {source} with index {source.Index} was not in the expected position in the collection. " +
                    $"Ensure that the sources passed to this collection when constructed are ordered by their Index.");
        }
    }
}
