using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    public enum MatchStatus
    {
        None,
        TestNodeNotFound,
        TestNodeFound
    }

    public interface IComparison
    {
        INode Control { get; }
        int ControlIndex { get; }
        INode? Test { get; }
        MatchStatus Status { get; }
    }
}
