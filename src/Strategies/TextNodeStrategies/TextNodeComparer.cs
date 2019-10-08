using System;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeComparer
    {
        private static readonly Regex WhitespaceReplace = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5));

        public WhitespaceOption Option { get; }

        public TextNodeComparer(WhitespaceOption option)
        {
            Option = option;
        }

        public CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSame() || currentDecision.IsSameAndBreak()) return currentDecision;
            if (Option != WhitespaceOption.Normalize) return currentDecision;

            if (comparison.Control.Node is IText controlTextNode && comparison.Test.Node is IText testTextNode)
            {
                var controlText = WhitespaceReplace.Replace(controlTextNode.Data.Trim(), " ");
                var testText = WhitespaceReplace.Replace(testTextNode.Data.Trim(), " ");

                if (controlText.Equals(testText, StringComparison.Ordinal)) return CompareResult.Same;
            }

            return currentDecision;
        }
    }
}
