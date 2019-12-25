using AngleSharp.Dom;

namespace AngleSharp.Diffing.Core
{
    public interface IDiffContext
    {
        IHtmlCollection<IElement> QueryControlRoot(string selector);
        IHtmlCollection<IElement> QueryTestRoot(string selector);
    }
}