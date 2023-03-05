namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

/// <summary>
/// Represents the class attribute comparer strategy.
/// </summary>
public static class ClassAttributeComparer
{
    private const string CLASS_ATTRIBUTE_NAME = "class";

    /// <summary>
    /// The class attribute comparer strategy.
    /// </summary>
    public static CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
    {
        if (currentDecision.IsSameOrSkip())
            return currentDecision;

        if (!IsBothClassAttributes(comparison))
            return CompareResult.Different;

        var (ctrlElm, testElm) = comparison.GetAttributeElements();
        if (ctrlElm.ClassList.Length != testElm.ClassList.Length)
            return CompareResult.Different;

        return ctrlElm.ClassList.All(x => testElm.ClassList.Contains(x))
            ? CompareResult.Same
            : CompareResult.Different;
    }

    private static bool IsBothClassAttributes(in AttributeComparison comparison)
    {
        return comparison.Control.Attribute.Name.Equals(CLASS_ATTRIBUTE_NAME, StringComparison.OrdinalIgnoreCase) &&
            comparison.Test.Attribute.Name.Equals(CLASS_ATTRIBUTE_NAME, StringComparison.OrdinalIgnoreCase);
    }

}
