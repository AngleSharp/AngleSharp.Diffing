using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Extensions
{
    public static class NodeExtensions
    {
        public static bool HasAttributes(this INode node)
        {
            return node is IElement element && element.Attributes.Length > 0;
        }
    }
}
