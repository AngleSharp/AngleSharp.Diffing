using System;
using AngleSharp.Dom;
using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Extensions;

namespace AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeFilter
    {
        private const string PRE_ELEMENTNAME = "PRE";
        private const string SCRIPT_ELEMENTNAME = "SCRIPT";
        private const string STYLE_ELEMENTNAME = "STYLE";
        private const string WHITESPACE_ATTR_NAME = "diff:whitespace";

        public WhitespaceOption Whitespace { get; }

        public TextNodeFilter(WhitespaceOption option)
        {
            Whitespace = option;
        }

        public FilterDecision Filter(in ComparisonSource source, FilterDecision currentDecision)
        {
            if (currentDecision.IsExclude()) return currentDecision;
            return source.Node is IText textNode ? Filter(textNode) : currentDecision;
        }

        private FilterDecision Filter(IText textNode)
        {
            var option = GetWhitespaceOption(textNode);
            return option != WhitespaceOption.Preserve && string.IsNullOrWhiteSpace(textNode.Data)
                ? FilterDecision.Exclude
                : FilterDecision.Keep;
        }

        private WhitespaceOption GetWhitespaceOption(IText textNode)
        {
            var parent = textNode.ParentElement;

            if (parent.NodeName.Equals(PRE_ELEMENTNAME, StringComparison.Ordinal) ||
                parent.NodeName.Equals(SCRIPT_ELEMENTNAME, StringComparison.Ordinal) ||
                parent.NodeName.Equals(STYLE_ELEMENTNAME, StringComparison.Ordinal))
            {
                return parent.TryGetAttrValue(WHITESPACE_ATTR_NAME, out WhitespaceOption option)
                    ? option
                    : WhitespaceOption.Preserve;
            }

            return parent.GetInlineOptionOrDefault(WHITESPACE_ATTR_NAME, Whitespace);
        }
    }
}
