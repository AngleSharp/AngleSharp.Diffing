<p align="center">
  <img width="400" src="/docs/header.png">
</p>

# AngleSharp Diffing - A diff/compare library for AngleSharp
[![Build status](https://ci.appveyor.com/api/projects/status/8awr3r4ylwy9habm?svg=true)](https://ci.appveyor.com/project/FlorianRappl/anglesharp-diffing)
[![GitHub Tag](https://img.shields.io/github/tag/AngleSharp/AngleSharp.Diffing.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.Diffing/releases)
[![NuGet Count](https://img.shields.io/nuget/dt/AngleSharp.Diffing.svg?style=flat-square)](https://www.nuget.org/packages/AngleSharp.Diffing/)
[![Issues Open](https://img.shields.io/github/issues/AngleSharp/AngleSharp.Diffing.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.Diffing/issues)
[![Gitter Chat](http://img.shields.io/badge/gitter-AngleSharp/AngleSharp-blue.svg?style=flat-square)](https://gitter.im/AngleSharp/AngleSharp)
[![CLA Assistant](https://cla-assistant.io/readme/badge/AngleSharp/AngleSharp.Diffing?style=flat-square)](https://cla-assistant.io/AngleSharp/AngleSharp.Diffing)

AngleSharp Diffing makes it possible to compare AngleSharp _control_ nodes and _test_ nodes and get a list of differences between them.

The _control_ nodes represents the expected HTML tree, i.e. how the nodes are expected to look, and the _test_ nodes represents the nodes that should be compared to the _control_ nodes.

See the [Wiki for documentation](wiki) and more examples.

**Differences:** There are three types off `IDiff` differences, that the library can return. 

- `NodeDiff`/`AttrDiff`: Represents a difference between a control and test node or a control and test attribute.
- `MissingNodeDiff`/`MissingAttrDiff`: Represents a difference where a control node or control attribute was expected to exist, but was not found in the test nodes tree.
- `UnexpectedNodeDiff`/`UnexpectedAttrDiff`: Represents a difference where a test node or test attribute was unexpectedly found in the test nodes tree, but did not have a match in the control nodes tree.

# Usage
To find the differences between a control HTML fragment and a test HTML fragment, using the default options, the easiest way is to use the `DiffBuilder` class, like so:

```csharp
var controlHtml = "<p>Hello World</p>";
var testHtml = "<p>World, I say hello</p>";
var diffs = DiffBuilder
    .Compare(control)
    .WithTest(test)
    .Build();
```

Read more about the available options on the [Options](/docs/Options.md) page.

# Documentation
- [Options](/docs/Options.md)
- [Creating custom diffing options](/docs/CustomOptions.md)
- [Difference engine internals](/docs/DiffingEngineInternals.md)

## Acknowledgments
- [Florian Rappl](https://github.com/FlorianRappl) from the AngleSharp team for providing ideas, input and sample code for working with AngleSharp. 
- [XMLUnit](https://www.xmlunit.org) has been a great inspiration for creating this library.
