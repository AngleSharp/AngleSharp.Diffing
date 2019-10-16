# AngleSharp Diffing - A diff/compare library for AngleSharp
This library makes it possible to compare a AngleSharp _control_ `INodeList` and a _test_ `INodeList` and get a list of `IDiff` differences between them.

The _control_ nodes represents the expected HTML tree, i.e. how the nodes are expected to look, and the _test_ nodes represents the nodes that should be compared to the _control_ nodes.

**Differences:** There are three types off `IDiff` differences, that the library can return. 

- `Diff`/`AttrDiff`: Represents a difference between a control and test node or a control and test attribute.
- `MissingDiff`/`MissingAttrDiff`: Represents a difference where a control node or control attribute was expected to exist, but was not found in the test nodes tree.
- `UnexpectedDiff`/`UnexpectedAttrDiff`: Represents a difference where a test node or test attribute was unexpectedly found in the test nodes tree, but did not have a match in the control nodes tree.

# Usage
To find the differences between a control HTML fragment and a test HTML fragment, using the default options, the easiest way is to use the `DiffBuilder` class, like so:

```csharp
var controlHtml = "<p>Hello World</p>";
var testHtml = "<p>World, I say hello</p>";
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .UseDefaultOptions()
    .Build();
```

Read more about the available options on the [Diffing Options/Strategies](/docs/Strategies.md) page.

# Documentation
- [Diffing Options/Strategies](/docs/Strategies.md)
- [Creating custom diffing options/strategies](/docs/CustomStrategies.md)
- [Difference engine internals](/docs/DifferenceEngineInternals.md)

## Acknowledgments
Big thanks to [Florian Rappl](https://github.com/FlorianRappl) from the AngleSharp team for providing ideas, input and sample code for working with AngleSharp. 

Another shout-out goes to [XMLUnit](https://www.xmlunit.org). It is a great XML diffing library, and it has been a great inspiration for creating this library.
