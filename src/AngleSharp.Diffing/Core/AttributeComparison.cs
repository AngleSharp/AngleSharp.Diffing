namespace AngleSharp.Diffing.Core;

/// <summary>
/// A match between two attributes that should be compared.
/// </summary>
public readonly record struct AttributeComparison(in AttributeComparisonSource Control, in AttributeComparisonSource Test)
{
}
