using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using Egil.AngleSharp.Diffing.Extensions;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeFilter
    {
        private const string PRE_ELEMENTNAME = "PRE";
        private const string WHITESPACE_ATTR_NAME = "diff:whitespace";

        public WhitespaceOption Whitespace { get; }

        public TextNodeFilter(WhitespaceOption option)
        {
            Whitespace = option;
        }

        public FilterDecision Filter(in ComparisonSource source, FilterDecision currentDecision)
        {
            if (currentDecision.IsExclude()) return currentDecision;
            return source.Node is IText textNode ? Filter(source, textNode) : currentDecision;
        }

        private FilterDecision Filter(in ComparisonSource source, IText textNode)
        {
            var option = GetWhitespaceOption(textNode);
            return option != WhitespaceOption.Preserve && string.IsNullOrWhiteSpace(textNode.Data)
                ? FilterDecision.Exclude
                : FilterDecision.Keep;
        }

        private WhitespaceOption GetWhitespaceOption(IText textNode)
        {
            var parent = textNode.ParentElement;

            if (parent.NodeName.Equals(PRE_ELEMENTNAME, StringComparison.Ordinal))
            {
                return parent.TryGetAttrValue(WHITESPACE_ATTR_NAME, out WhitespaceOption option)
                    ? option
                    : WhitespaceOption.Preserve;
            }

            return parent.GetInlineOptionOrDefault(WHITESPACE_ATTR_NAME, Whitespace);
        }
    }
}
