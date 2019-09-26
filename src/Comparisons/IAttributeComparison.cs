namespace Egil.AngleSharp.Diffing.Comparisons
{
    public interface IAttributeComparison
    {
        IAttributeComparisonSource Control { get; }
        IAttributeComparisonSource Test { get; }
    }
}
