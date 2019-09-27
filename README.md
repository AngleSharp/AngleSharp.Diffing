# AngleSharp.Diffing 
This library makes it possible to compare a AngleSharp _control_ `INodeList` and a _test_ `INodeList` and get a list of differences between them.

The _control_ nodes represents the expected, i.e. how the nodes are expected to look, and the _test_ nodes represents the other nodes 
that should be compared to the _control_ nodes.

## Usage

## Difference engine steps
The heart of the library is the `HtmlDifferenceEngine`, which goes through the following steps to determine if the 
control `INodeList` is the same as the test `INodeList`.