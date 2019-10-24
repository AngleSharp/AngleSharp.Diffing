using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public class BooleanAttributeComparer
    {
        private static readonly HashSet<string> BooleanAttributesSet = new HashSet<string>()
        {
            "allowfullscreen",
            "allowpaymentrequest",
            "async",
            "autofocus",
            "autoplay",
            "checked",
            "controls",
            "default",
            "defer",
            "disabled",
            "formnovalidate",
            "hidden",
            "ismap",
            "itemscope",
            "loop",
            "multiple",
            "muted",
            "nomodule",
            "novalidate",
            "open",
            "readonly",
            "required",
            "reversed",
            "selected",
            "typemustmatch"
        };

        public static IReadOnlyCollection<string> BooleanAttributes => BooleanAttributesSet;

        private readonly BooleanAttributeComparision _mode;

        public BooleanAttributeComparer(BooleanAttributeComparision mode)
        {
            _mode = mode;
        }

        public CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSameOrSkip()) return currentDecision;
            if (!IsAttributeNamesEqual(comparison)) return CompareResult.Different;
            if (!BooleanAttributesSet.Contains(comparison.Control.Attribute.Name)) return currentDecision;

            var hasSameValue = _mode == BooleanAttributeComparision.Strict
                ? CompareStrict(comparison)
                : true;

            return hasSameValue ? CompareResult.Same : CompareResult.Different;
        }

        private static bool IsAttributeNamesEqual(in AttributeComparison comparison)
        {
            return comparison.Control.Attribute.Name.Equals(comparison.Test.Attribute.Name, StringComparison.OrdinalIgnoreCase);
        }

        private static bool CompareStrict(in AttributeComparison comparison)
        {
            return IsAttributeStrictlyTruthy(comparison.Control.Attribute) && IsAttributeStrictlyTruthy(comparison.Test.Attribute);
        }

        private static bool IsAttributeStrictlyTruthy(IAttr attr)
            => string.IsNullOrWhiteSpace(attr.Value) || attr.Value.Equals(attr.Name, StringComparison.OrdinalIgnoreCase);
    }
}
