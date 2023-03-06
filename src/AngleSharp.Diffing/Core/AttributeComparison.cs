namespace AngleSharp.Diffing.Core;

/// <summary>
/// A match between two attributes that should be compared.
/// </summary>
public readonly struct AttributeComparison : IEquatable<AttributeComparison>
{
    /// <summary>
    /// Gets the control attribute which the <see cref="Test"/> attribute is supposed to match.
    /// </summary>
    public AttributeComparisonSource Control { get; }

    /// <summary>
    /// Gets the test attribute which should be compared to the <see cref="Control"/> attribute.
    /// </summary>
    public AttributeComparisonSource Test { get; }

    /// <summary>
    /// Create a attribute comparison match.
    /// </summary>
    /// <param name="control">The attribute control source</param>
    /// <param name="test">The attribute test source</param>
    public AttributeComparison(in AttributeComparisonSource control, in AttributeComparisonSource test)
    {
        Control = control;
        Test = test;
    }

    /// <summary>
    /// Returns the control and test elements which the control and test attributes belongs to.
    /// </summary>
    public (IElement ControlElement, IElement TestElement) GetAttributeElements()
        => ((IElement)Control.ElementSource.Node, (IElement)Test.ElementSource.Node);

    #region Equals and HashCode
    /// <inheritdoc/>
    public bool Equals(AttributeComparison other) => Control.Equals(other.Control) && Test.Equals(other.Test);
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AttributeComparison other && Equals(other);
    /// <inheritdoc/>
    public override int GetHashCode() => (Control, Test).GetHashCode();
    /// <inheritdoc/>
    public static bool operator ==(AttributeComparison left, AttributeComparison right) => left.Equals(right);
    /// <inheritdoc/>
    public static bool operator !=(AttributeComparison left, AttributeComparison right) => !left.Equals(right);
    #endregion
}
