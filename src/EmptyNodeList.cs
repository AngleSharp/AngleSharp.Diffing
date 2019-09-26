using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    public class EmptyNodeList : INodeList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public INode this[int index] => throw new IndexOutOfRangeException("There are no nodes in an empty node list");

        public int Length => 0;

        private EmptyNodeList() { }

        public IEnumerator<INode> GetEnumerator() { yield break; }

        public void ToHtml(TextWriter writer, IMarkupFormatter formatter) { }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static readonly INodeList Instance = new EmptyNodeList();
    }
}
