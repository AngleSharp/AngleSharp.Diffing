using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Core;
using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public static class StyleAttributeComparer
    {        
        public static CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSameOrSkip()) return currentDecision;

            return IsStyleAttributeComparison(comparison)
                ? CompareElementStyle(comparison)
                : currentDecision;
        }

        private static CompareResult CompareElementStyle(in AttributeComparison comparison)
        {
            var (ctrlElm, testElm) = comparison.GetAttributeElements();
            var ctrlStyle = ctrlElm.GetStyle();
            var testStyle = testElm.GetStyle();
            return ctrlStyle.CssText.Equals(testStyle.CssText, StringComparison.Ordinal)
                ? CompareResult.Same
                : CompareResult.Different;
        }

        private static bool IsStyleAttributeComparison(in AttributeComparison comparison)
        {
            return comparison.Control.Attribute.Name.Equals(AttributeNames.Style, StringComparison.Ordinal) && 
                comparison.Test.Attribute.Name.Equals(AttributeNames.Style, StringComparison.Ordinal);
        }
    }
}
