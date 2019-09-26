using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing
{
    public interface IDiff
    {
        DiffResult Result { get; }
        NodeType Target { get; }
    }
}
