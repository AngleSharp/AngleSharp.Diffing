using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Core
{
    public delegate FilterDecision SourceMapRemovePredicate(in AttributeComparisonSource source);

    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
    public class SourceMap : IEnumerable<AttributeComparisonSource>
    {
        private readonly HashSet<string> _matched = new HashSet<string>();
        private readonly Dictionary<string, AttributeComparisonSource> _sources = new Dictionary<string, AttributeComparisonSource>();

        public ComparisonSourceType SourceType { get; }

        public int Count => _sources.Count;

        public AttributeComparisonSource this[string name]
        {
            get
            {
                return _sources[name];
            }
        }

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

        public bool Contains(string name) => _sources.ContainsKey(name);

        public bool IsUnmatched(string name) => !_matched.Contains(name);

        public IEnumerable<AttributeComparisonSource> GetUnmatched()
        {
            foreach (var source in _sources.Values)
            {
                if (!_matched.Contains(source.Attribute.Name)) yield return source;
            }
        }

        public IEnumerator<AttributeComparisonSource> GetEnumerator() => _sources.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void Remove(SourceMapRemovePredicate predicate)
        {
            var removeQueue = new Queue<string>(Count);
            foreach (var source in _sources.Values)
            {
                if (predicate(source) == FilterDecision.Exclude) removeQueue.Enqueue(source.Attribute.Name);
            }
            foreach (var name in removeQueue)
            {
                _sources.Remove(name);
            }
        }

        internal void MarkAsMatched(in AttributeComparisonSource source)
        {
            _matched.Add(source.Attribute.Name);
        }
    }
}