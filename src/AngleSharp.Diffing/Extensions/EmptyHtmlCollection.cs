using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Extensions
{
    public class EmptyHtmlCollection<T> : IHtmlCollection<T> where T : IElement
    {
        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        public T this[int index] => throw new IndexOutOfRangeException();

        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        public T this[string id] => throw new IndexOutOfRangeException();

        public int Length => 0;

        public IEnumerator<T> GetEnumerator() { yield break; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static readonly IHtmlCollection<T> Empty = new EmptyHtmlCollection<T>();
    }
}
