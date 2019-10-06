using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.AngleSharp.Diffing.Strategies
{
    public class IgnoreAttributeTest
    {
        // When a control element with diff:ignore not matched, it does not count as a missing diff
        // When a control attribute with :ignore postfix is not matched, it does not count as a missing attr diff
    }
}
