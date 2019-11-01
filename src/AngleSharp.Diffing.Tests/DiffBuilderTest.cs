using System;
using Egil.AngleSharp.Diffing.Strategies;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{

    public class DiffBuilderTest
    {
        private DiffingStrategyPipeline DefaultStrategy { get; } = new DiffingStrategyPipelineBuilder().WithDefaultOptions().Build();

        [Fact(DisplayName = "Control and test html are set correctly")]
        public void Test001()
        {
            var control = "<p>control</p>";
            var test = "<p>test</p>";

            var sut = new DiffBuilder(DefaultStrategy)
                .Compare(control)
                .WithTest(test);

            sut.Control.ShouldBe(control);
            sut.Test.ShouldBe(test);
        }

        [Fact(DisplayName = "Builder throws if null is passed to control and test")]
        public void Test002()
        {
            Should.Throw<ArgumentNullException>(() => new DiffBuilder(DefaultStrategy).Compare(null!)).ParamName.ShouldBe(nameof(DiffBuilder.Control));
            Should.Throw<ArgumentNullException>(() => new DiffBuilder(DefaultStrategy).Compare("").WithTest(null!)).ParamName.ShouldBe(nameof(DiffBuilder.Test));
        }

        [Fact(DisplayName = "Creating DiffBuilder with null strategies throws")]
        public void Test3()
        {
            Should.Throw<ArgumentNullException>(() => new DiffBuilder(null!));
        }

        [Fact(DisplayName = "Calling Build() with DefaultOptions() returns expected diffs")]
        public void Test6()
        {
            var control = "<p>hello <em>world</em></p>";
            var test = "<p>world says <strong>hello</strong></p>";

            var diffs = new DiffBuilder(DefaultStrategy)
                .Compare(control)
                .WithTest(test)
                .Build();

            diffs.ShouldNotBeEmpty();
        }

        [Fact(DisplayName = "Test")]
        public void MyTestMethod()
        {
            var control = "\r\n        <h1>Hello world</h1>\r\n        <h1>Hello world</h1>\r\n    ";
            var test = "\r\n        <h1>Hello world</h1>\r\n        <h1>Hello world</h1>\r\n    ";

            var diffs = new DiffBuilder(DefaultStrategy)
                .Compare(control)
                .WithTest(test)
                .Build();

            diffs.ShouldBeEmpty();
        }
    }
}
