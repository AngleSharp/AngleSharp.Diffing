namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a difference between two texts.
/// </summary>
public class TextDiff : NodeDiff
{
    /// <summary>
    /// Creates a <see cref="NodeDiff"/>.
    /// </summary>
    public TextDiff(in Comparison comparison) : base(comparison)
    {
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Text diff: Control = {Control.Path}, Test = {Test.Path}";
    }
}
