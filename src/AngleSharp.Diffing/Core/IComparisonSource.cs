namespace Egil.AngleSharp.Diffing.Core
{
    public interface IComparisonSource
    {
        string Path { get; }
        ComparisonSourceType SourceType { get; }
    }
}