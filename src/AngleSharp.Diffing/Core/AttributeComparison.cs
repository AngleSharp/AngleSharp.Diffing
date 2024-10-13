namespace AngleSharp.Diffing.Core;

/// <summary>
/// A match between two attributes that should be compared.
/// </summary>
/// <param name="Control">Gets the control attribute which the <see cref="Test"/> attribute is supposed to match.</param>
/// <param name="Test">Gets the test attribute which should be compared to the <see cref="Control"/> attribute.</param>
public readonly record struct AttributeComparison(in AttributeComparisonSource Control, in AttributeComparisonSource Test)
{
    /// <summary>
    /// Returns the control and test elements which the control and test attributes belongs to.
    /// </summary>
    public readonly (IElement ControlElement, IElement TestElement) AttributeElements
        => ((IElement)Control.ElementSource.Node, (IElement)Test.ElementSource.Node);
}
