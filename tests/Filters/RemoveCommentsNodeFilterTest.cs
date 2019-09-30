using AngleSharp.Dom;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.AngleSharp.Diffing.Filters
{
    public class RemoveCommentsNodeFilterTest : DiffingTestBase
    {
        [Fact(DisplayName = "Filter removes comment nodes and leaves other nodes alone")]
        public void Test1()
        {
            var comment = ToComparisonSource<IComment>("<!--comment-->");
            var sut = new RemoveCommentsNodeFilter();

            sut.Filter(comment, true).ShouldBe(false);
        }
    }
}
