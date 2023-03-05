namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

/// <summary>
/// Boolean attribute comparer strategy.
/// </summary>
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

    /// <summary>
    /// Gets a collection of names of all attributes the strategy considers as boolean attributes.
    /// </summary>        
    public static IReadOnlyCollection<string> BooleanAttributes => BooleanAttributesSet;

    private readonly BooleanAttributeComparision _mode;

    /// <summary>
    /// Creates a instance of the <see cref="BooleanAttributeComparer"/>.
    /// </summary>
    /// <param name="mode"></param>
    public BooleanAttributeComparer(BooleanAttributeComparision mode)
    {
        _mode = mode;
    }

    /// <summary>
    /// The boolean attribute comparer strategy.
    /// </summary>
    public CompareResult Compare(in AttributeComparison comparison, CompareResult currentDecision)
    {
        if (currentDecision.IsSameOrSkip())
            return currentDecision;
        if (!IsAttributeNamesEqual(comparison))
            return CompareResult.Different;
        if (!BooleanAttributesSet.Contains(comparison.Control.Attribute.Name))
            return currentDecision;

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
