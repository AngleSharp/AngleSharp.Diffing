using System;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;
using Egil.AngleSharp.Diffing.Extensions;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeComparer
    {
        private const string PRE_ELEMENTNAME = "PRE";
        private const string WHITESPACE_ATTR_NAME = "diff:whitespace";
        private const string IGNORECASE_ATTR_NAME = "diff:ignorecase";
        private static readonly Regex WhitespaceReplace = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5));

        public WhitespaceOption Whitespace { get; }

        public bool IgnoreCase { get; }

        public TextNodeComparer(WhitespaceOption option = WhitespaceOption.Preserve, bool ignoreCase = false)
        {
            Whitespace = option;
            IgnoreCase = ignoreCase;
        }

        public CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSame() || currentDecision.IsSameAndBreak())
                return currentDecision;
            if (comparison.Control.Node is IText controlTextNode && comparison.Test.Node is IText testTextNode)
                return Compare(controlTextNode, testTextNode, currentDecision);
            else
                return currentDecision;
        }

        private CompareResult Compare(IText controlTextNode, IText testTextNode, CompareResult currentDecision)
        {
            var option = GetWhitespaceOption(controlTextNode);
            var compareMethod = GetCompareMethod(controlTextNode);
            var controlText = controlTextNode.Data;
            var testText = testTextNode.Data;

            if (option == WhitespaceOption.Normalize)
            {
                controlText = WhitespaceReplace.Replace(controlText.Trim(), " ");
                testText = WhitespaceReplace.Replace(controlText.Trim(), " ");
            }            

            if (controlText.Equals(testText, compareMethod))
                return CompareResult.Same;
            else
                return currentDecision;
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

            return parent.GetInlineOptionOrDefault<WhitespaceOption>(WHITESPACE_ATTR_NAME, Whitespace);
        }

        private StringComparison GetCompareMethod(IText controlTextNode)
        {
            return controlTextNode.ParentElement.GetInlineOptionOrDefault(IGNORECASE_ATTR_NAME, IgnoreCase)
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;
        }
    }
}
