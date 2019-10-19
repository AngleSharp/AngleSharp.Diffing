using System;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
    public class DiffBuilderTest
    {
        [Fact(DisplayName = "Control and test html are set correctly")]
        public void Test001()
        {
            var control = "<p>control</p>";
            var test = "<p>test</p>";

            var sut = DiffBuilder
                .Compare(control)
                .WithTest(test);

            sut.Control.ShouldBe(control);
            sut.Test.ShouldBe(test);
        }

        [Fact(DisplayName = "Builder throws if null is passed to control and test")]
        public void Test002()
        {
            Should.Throw<ArgumentNullException>(() => DiffBuilder.Compare(null!)).ParamName.ShouldBe(nameof(DiffBuilder.Control));
            Should.Throw<ArgumentNullException>(() => DiffBuilder.Compare("").WithTest(null!)).ParamName.ShouldBe(nameof(DiffBuilder.Test));
        }

        //[Fact(DisplayName = "Calling .Build() returns expected diffs")]
        //public void Test3()
        //{
        //    var control = "<p>hello <em>world</em></p>";
        //    var test = "<p>world says <strong>hello</strong></p>";

        //    var diffs = DiffBuilder.Compare(control).WithTest(test).Build();

        //    diffs.ShouldNotBeEmpty());
        //}
    }
}
