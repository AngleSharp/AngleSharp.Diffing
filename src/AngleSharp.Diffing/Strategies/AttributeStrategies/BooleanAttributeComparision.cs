namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

/// <summary>
/// Controls the type of comparison performed by the <see cref="BooleanAttributeComparer"/>.
/// </summary>
public enum BooleanAttributeComparision
{
    /// <summary>
    /// Tells the <see cref="BooleanAttributeComparer"/> to just assert that a boolean attribute exists, independent of its value.
    /// </summary>
    Loose,
    /// <summary>
    /// Tells the <see cref="BooleanAttributeComparer"/> to verify that the attribute is both a boolean attribute and that its value is legal.
    /// </summary>
    Strict
}
