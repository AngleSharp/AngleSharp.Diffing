using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Egil.AngleSharp.Diffing.Core
{
    public delegate FilterDecision SourceCollectionRemovePredicate(in ComparisonSource source);

    public class SourceCollection : IEnumerable<ComparisonSource>
    {
        private const int SOURCE_REMOVED = -1;
        private const int SOURCE_UNMATCHED = 0;

        private readonly int[] _status;
        private ComparisonSource[] _sources;

        public ComparisonSourceType SourceType { get; }

        public int Count { get; private set; }

        public ComparisonSource this[int index]
        {
            get => _sources[index];
        }

        public SourceCollection(ComparisonSourceType sourceType, IEnumerable<ComparisonSource> sources)
        {
            SourceType = sourceType;
            _sources = sources.ToArray();
            _status = new int[_sources.Length];
            Count = _sources.Length;
            EnsureSourcesAreInCorrectOrder();
        }

        public IEnumerable<ComparisonSource> GetUnmatched()
        {
            for (int i = 0; i < _sources.Length; i++)
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
                yield return _sources[i];
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void MarkAsMatched(in ComparisonSource source)
        {
            if (_status[source.Index] == SOURCE_REMOVED)
                throw new InvalidOperationException("A removed source cannot be marked as matched. The source is not supposed to be part of the comparison.");

            _status[source.Index]++;
        }

        internal void Remove(SourceCollectionRemovePredicate predicate)
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
            var oldSources = _sources;
            _sources = Count == 0 ? Array.Empty<ComparisonSource>() : new ComparisonSource[Count];
            if (Count > 0)
            {
                for (int i = 0, j = 0; i < oldSources.Length; i++)
                {
                    if (_status[i] != SOURCE_REMOVED)
                    {
                        _sources[j++] = oldSources[i];
                    }
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
