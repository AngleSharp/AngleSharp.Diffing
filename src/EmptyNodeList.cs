using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    public class EmptyNodeList : INodeList
    {
        public INode this[int index] => throw new IndexOutOfRangeException("There are no nodes in an empty node list");

        public int Length => 0;

        private EmptyNodeList() { }

        public IEnumerator<INode> GetEnumerator() { yield break; }

        public void ToHtml(TextWriter writer, IMarkupFormatter formatter) { }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static readonly INodeList Instance = new EmptyNodeList();
    }
}
