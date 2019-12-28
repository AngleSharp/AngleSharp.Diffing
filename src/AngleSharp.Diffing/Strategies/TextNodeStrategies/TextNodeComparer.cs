using System;
using System.Text.RegularExpressions;

using AngleSharp.Diffing.Core;
using AngleSharp.Diffing.Extensions;
using AngleSharp.Dom;

namespace AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeComparer
    {
        private static readonly string[] DefaultPreserveElement = new string[] { "PRE", "SCRIPT", "STYLE" };
        private const string WHITESPACE_ATTR_NAME = "diff:whitespace";
        private const string IGNORECASE_ATTR_NAME = "diff:ignorecase";
        private const string REGEX_ATTR_NAME = "diff:regex";
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
            if (currentDecision.IsSameOrSkip()) return currentDecision;

            if (comparison.TryGetNodesAsType<IText>(out var controlTextNode, out var testTextNode))
                return Compare(controlTextNode, testTextNode);
            else
                return currentDecision;
        }

        private CompareResult Compare(IText controlTextNode, IText testTextNode)
        {
            var option = GetWhitespaceOption(controlTextNode);
            var compareMethod = GetCompareMethod(controlTextNode);
            var controlText = controlTextNode.Data;
            var testText = testTextNode.Data;

            if (option == WhitespaceOption.Normalize)
            {
                controlText = WhitespaceReplace.Replace(controlText.Trim(), " ");
                testText = WhitespaceReplace.Replace(testText.Trim(), " ");
            }

            var isRegexCompare = GetIsRegexComparison(controlTextNode);

            return isRegexCompare
                ? PerformRegexCompare(compareMethod, controlText, testText)
                : PerformStringCompare(compareMethod, controlText, testText);
        }

        private static CompareResult PerformRegexCompare(StringComparison compareMethod, string controlText, string testText)
        {
            var regexOptions = compareMethod == StringComparison.OrdinalIgnoreCase
                ? RegexOptions.IgnoreCase
                : RegexOptions.None;

            return Regex.IsMatch(testText, controlText, regexOptions, TimeSpan.FromSeconds(5))
                ? CompareResult.Same
                : CompareResult.Different;
        }

        private static CompareResult PerformStringCompare(StringComparison compareMethod, string controlText, string testText)
        {
            return controlText.Equals(testText, compareMethod)
                ? CompareResult.Same
                : CompareResult.Different;
        }

        private static bool GetIsRegexComparison(IText controlTextNode)
        {
            var parent = controlTextNode.ParentElement;
            return parent is { } && parent.TryGetAttrValue(REGEX_ATTR_NAME, out bool isRegex) && isRegex;
        }

        private WhitespaceOption GetWhitespaceOption(IText textNode)
        {
            var parent = textNode.ParentElement;
            foreach (var tagName in DefaultPreserveElement)
            {
                if (parent.NodeName.Equals(tagName, StringComparison.Ordinal))
                {
                    return parent.TryGetAttrValue(WHITESPACE_ATTR_NAME, out WhitespaceOption option)
                        ? option
                        : WhitespaceOption.Preserve;
                }
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
