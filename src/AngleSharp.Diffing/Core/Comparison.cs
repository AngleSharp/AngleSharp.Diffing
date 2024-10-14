using System.Runtime.InteropServices;

namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represent a comparison between two nodes.
/// </summary>
/// <param name="Control">Gets the control source in the comparison.</param>
/// <param name="Test">Gets the test source in the comparison.</param>
[StructLayout(LayoutKind.Auto)]
public readonly record struct Comparison(in ComparisonSource Control, in ComparisonSource Test)
{
    /// <summary>
    /// Gets whether the control and test nodes are of the same type and has the same name.
    /// </summary>
    public bool AreNodeTypesEqual
        => Control.Node.NodeType == Test.Node.NodeType
        && Control.Node.NodeName.Equals(Test.Node.NodeName, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Try to get the control and test node as the <typeparamref name="TNode"/> type.
    /// Returns true if both test and control node is of type <typeparamref name="TNode"/>, false otherwise.
    /// </summary>
    /// <typeparam name="TNode">THe type of node to try to retrieve.</typeparam>
    /// <returns>Returns true if both test and control node is of type <typeparamref name="TNode"/>, false otherwise.</returns>
    public bool TryGetNodesAsType<TNode>([NotNullWhen(true)]out TNode? controlNode, [NotNullWhen(true)]out TNode? testNode) where TNode : class, INode
    {
        if (Control.Node is TNode ctrl && Test.Node is TNode test)
        {
            controlNode = ctrl;
            testNode = test;
            return true;
        }
        else
        {
            controlNode = default;
            testNode = default;
            return false;
        }
    }
}
