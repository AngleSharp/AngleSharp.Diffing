using System;
using System.Diagnostics;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Core
{

    [DebuggerDisplay("Diff={Target} {Result} Control={Control.Node.NodeName}[{Control.Index}]")]
    public class MissingNodeDiff : MissingDiffBase<ComparisonSource>
    {
        public MissingNodeDiff(in ComparisonSource control) : base(control, control.Node.NodeType.ToDiffTarget())
        {
        }
    }
}
