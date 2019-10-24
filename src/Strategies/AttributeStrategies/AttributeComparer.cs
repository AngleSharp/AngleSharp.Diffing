using System;
using System.Text.RegularExpressions;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public static class AttributeComparer
    {
        private const string IGNORE_CASE_POSTFIX = ":ignorecase";
        private const string REGEX_POSTFIX = ":regex";
        private const string IGNORE_CASE_REGEX_POSTFIX = IGNORE_CASE_POSTFIX + REGEX_POSTFIX;
        private const string REGEX_IGNORE_CASE_POSTFIX = REGEX_POSTFIX + IGNORE_CASE_POSTFIX;

        public static CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSameOrSkip()) return currentDecision;

            var (ignoreCase, isRegexValueCompare) = GetComparisonModifiers(comparison);

            var hasSameName = CompareAttributeNames(comparison, ignoreCase, isRegexValueCompare);

            if(!hasSameName) return CompareResult.Different;

            var hasSameValue = isRegexValueCompare
                ? CompareAttributeValuesByRegex(comparison, ignoreCase)
                : CompareAttributeValues(comparison, ignoreCase);

            return hasSameValue
                ? CompareResult.Same
                : CompareResult.Different;
        }

        private static (bool ignoreCase, bool isRegexCompare) GetComparisonModifiers(in AttributeComparison comparison)
        {
            var ctrlName = comparison.Control.Attribute.Name;
            if (ctrlName.EndsWith(IGNORE_CASE_REGEX_POSTFIX, StringComparison.Ordinal) || ctrlName.EndsWith(REGEX_IGNORE_CASE_POSTFIX, StringComparison.Ordinal))
                return (ignoreCase: true, isRegexCompare: true);
            else if (ctrlName.EndsWith(IGNORE_CASE_POSTFIX, StringComparison.Ordinal))
                return (ignoreCase: true, isRegexCompare: false);
            else if (ctrlName.EndsWith(REGEX_POSTFIX, StringComparison.Ordinal))
                return (ignoreCase: false, isRegexCompare: true);
            else
                return (ignoreCase: false, isRegexCompare: false);
        }

        private static bool CompareAttributeValues(in AttributeComparison comparison, bool ignoreCase)
        {
            var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return comparison.Control.Attribute.Value.Equals(comparison.Test.Attribute.Value, comparisonType);
        }

        private static bool CompareAttributeValuesByRegex(in AttributeComparison comparison, bool ignoreCase)
        {
            var comparisonType = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            return Regex.IsMatch(comparison.Test.Attribute.Value, comparison.Control.Attribute.Value, comparisonType, TimeSpan.FromSeconds(5));
        }

        private static bool CompareAttributeNames(in AttributeComparison comparison, bool hasIgnorePostfix, bool hasRegexPostfix)
        {
            var ctrlName = comparison.Control.Attribute.Name;

            if (hasIgnorePostfix && !hasRegexPostfix)
                ctrlName = ctrlName.Substring(0, ctrlName.Length - IGNORE_CASE_POSTFIX.Length);
            else if (hasRegexPostfix && !hasIgnorePostfix)
                ctrlName = ctrlName.Substring(0, ctrlName.Length - REGEX_POSTFIX.Length);
            else if (hasIgnorePostfix && hasRegexPostfix)
                ctrlName = ctrlName.Substring(0, ctrlName.Length - IGNORE_CASE_REGEX_POSTFIX.Length);

            return ctrlName.Equals(comparison.Test.Attribute.Name, StringComparison.Ordinal);
        }
    }
}
