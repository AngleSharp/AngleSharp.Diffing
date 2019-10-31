using System;
using System.Linq;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public static class ClassAttributeComparer
    {
        private const string CLASS_ATTRIBUTE_NAME = "class";

        public static CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsSameOrSkip()) return currentDecision;
            if (!IsClassAttributes(comparison)) return currentDecision;

            var (ctrlElm, testElm) = comparison.GetAttributeElements();            
            if (ctrlElm.ClassList.Length != testElm.ClassList.Length) return CompareResult.Different;

            return ctrlElm.ClassList.All(x => testElm.ClassList.Contains(x))
                ? CompareResult.Same
                : CompareResult.Different;
        }

        private static bool IsClassAttributes(in AttributeComparison comparison)
        {
            return comparison.Control.Attribute.Name.Equals(CLASS_ATTRIBUTE_NAME, StringComparison.OrdinalIgnoreCase) &&
                comparison.Test.Attribute.Name.Equals(CLASS_ATTRIBUTE_NAME, StringComparison.OrdinalIgnoreCase);
        }

    }
}
