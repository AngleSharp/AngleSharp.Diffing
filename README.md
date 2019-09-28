# AngleSharp.Diffing 
This library makes it possible to compare a AngleSharp _control_ `INodeList` and a _test_ `INodeList` and get a list of `IDiff` differences between them.

The _control_ nodes represents the expected, i.e. how the nodes are expected to look, and the _test_ nodes represents the other nodes that should be compared to the _control_ nodes.

## Usage



## Difference engine details
The heart of the library is the `HtmlDifferenceEngine` class, which goes through the steps illustrated in the activity diagram below to determine if the control nodes is the same as the test nodes.

The `HtmlDifferenceEngine` class depends on three _strategies_, the `IFilterStrategy`, `IMatcherStrategy`, and `ICompareStrategy` types. These are used in the highlighted activities in the diagram. With those, we can control what nodes and attributes take part in the comparison (filter strategy), how control and test nodes and attributes are matched up for comparison (matching strategy), and finally, how nodes and attributes are determined to be same or different (compare strategy).

It starts with a call to the `Compare(INodeList controlNodes, INodeList testNodes)` and recursively calls itself when nodes have child nodes.

![img](docs/HtmlDifferenceEngineFlow.svg)

The library comes with a bunch of different filters, matchers, and conparers, that you can configure and mix and match with your own, to get the exact diffing experience you want. See the Usage section above for details.

## Creating custom diffing strategies

## Acknowledgement
Big thanks to [Florian Rappl](https://github.com/FlorianRappl) from the AngleSharp team for providing ideas, input and sample code for working with AngleSharp. 

Another shout-out goes to [XMLUnit](https://www.xmlunit.org). It is a great XML diffing library, and it has been a great inspiration for creating this library.
