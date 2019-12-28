using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AngleSharp.Dom;

namespace AngleSharp.Diffing.Extensions
{
    /// <summary>
    /// Represents an empty <see cref="IHtmlCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EmptyHtmlCollection<T> : IHtmlCollection<T> where T : IElement
    {
        /// <inheritdoc/>
        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        public T this[int index] => throw new IndexOutOfRangeException();

        /// <inheritdoc/>
        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
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
}
