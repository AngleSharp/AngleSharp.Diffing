using AngleSharp.Dom;

namespace AngleSharp.Diffing.Core
{
    public enum DiffTarget
    {
        None,
        Attribute,
        Comment,
        Element,
        Node,
        Text
    }

    public static class NodeTypeExtensions
    {
        public static DiffTarget ToDiffTarget(this NodeType nodeType)
        {
            return nodeType switch
            {
                NodeType.Element => DiffTarget.Element,
                NodeType.Comment => DiffTarget.Comment,
                NodeType.Text => DiffTarget.Text,
                _ => DiffTarget.Node
            };
        }
    }

}
