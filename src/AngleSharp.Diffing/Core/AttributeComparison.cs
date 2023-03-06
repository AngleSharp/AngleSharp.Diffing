namespace AngleSharp.Diffing.Core;

/// <summary>
/// A match between two attributes that should be compared.
/// </summary>
public readonly record struct AttributeComparison(in AttributeComparisonSource Control, in AttributeComparisonSource Test)
{
    /// <summary>
    /// Returns the control and test elements which the control and test attributes belongs to.
    /// </summary>
    public (IElement ControlElement, IElement TestElement) GetAttributeElements()
        => ((IElement)Control.ElementSource.Node, (IElement)Test.ElementSource.Node);
}
