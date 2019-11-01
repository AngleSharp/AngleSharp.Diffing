using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AngleSharp.Diffing.Core
{
    public delegate FilterDecision SourceCollectionRemovePredicate(in ComparisonSource source);

    public class SourceCollection : IEnumerable<ComparisonSource>
    {
        private const int SOURCE_REMOVED = -1;
        private const int SOURCE_UNMATCHED = 0;
        private const int SOURCE_MATCHED = 1;

        private readonly int[] _status;
        private ComparisonSource[] _sources;

        public ComparisonSourceType SourceType { get; }

        public int Count { get; private set; }

        public ComparisonSource this[int index]
        {
            get
            {
                if (_status[_sources[index].Index] == SOURCE_REMOVED)
                    throw new InvalidOperationException("The source at the specified index has been removed.");
                return _sources[index];
            }
        }

        public SourceCollection(ComparisonSourceType sourceType, IEnumerable<ComparisonSource> sources)
        {
            SourceType = sourceType;
            _sources = sources.ToArray();
            _status = new int[_sources.Length];
            Count = _sources.Length;
            EnsureSourcesAreInCorrectOrder();
        }

        public IEnumerable<ComparisonSource> GetUnmatched(int startIndex = 0)
        {
            for (int i = startIndex; i < _sources.Length; i++)
            {
                if (_status[_sources[i].Index] == SOURCE_UNMATCHED)
                {
                    yield return _sources[i];
                }
            }
            yield break;
        }

        public IEnumerator<ComparisonSource> GetEnumerator()
        {
            for (int i = 0; i < _sources.Length; i++)
            {
                if (_status[_sources[i].Index] != SOURCE_REMOVED)
                {
                    yield return _sources[i];
                }
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets all the sources originally in the collection, even those removed and those marked as matched.
        /// </summary>
        public IEnumerable<ComparisonSource> GetAllSources() => _sources;

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
            for (int i = 0; i < _sources.Length; i++)
            {
                var source = _sources[i];
                if (predicate(source) == FilterDecision.Exclude)
                {
                    _status[source.Index] = SOURCE_REMOVED;
                    Count--;
                }
            }
        }

        private void EnsureSourcesAreInCorrectOrder()
        {
            for (int i = 0; i < _sources.Length; i++)
            {
                while (_sources[i].Index != i)
                {
                    var tmp = _sources[i];
                    _sources[i] = _sources[tmp.Index];
                    _sources[tmp.Index] = tmp;
                }
            }
        }
    }
}