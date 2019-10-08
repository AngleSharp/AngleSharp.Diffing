# AngleSharp Diffing - A diff/compare library for AngleSharp
This library makes it possible to compare a AngleSharp _control_ `INodeList` and a _test_ `INodeList` and get a list of `IDiff` differences between them.

The _control_ nodes represents the expected HTML tree, i.e. how the nodes are expected to look, and the _test_ nodes represents the nodes that should be compared to the _control_ nodes.

**Differences:** There are three types off `IDiff` differences, that the library can return. 

- `Diff`/`AttrDiff`: Represents a difference between a control and test node or a control and test attribute.
- `MissingDiff`/`MissingAttrDiff`: Represents a difference where a control node or control attribute was expected to exist, but was not found in the test nodes tree.
- `UnexpectedDiff`/`UnexpectedAttrDiff`: Represents a difference where a test node or test attribute was unexpectedly found in the test nodes tree, but did not have a match in the control nodes tree.

## Usage
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

The `DiffBuilder` class handles the relative complex task of setting up the `HtmlDifferenceEngine`.

Using the `UseDefaultOptions()` method is equivalent to setting the following options explicitly:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .IgnoreComments()
    .Whitespace(WhitespaceOption.Normalize)
    .IgnoreDiffAttributes()
    .Build();
``` 

## Diffing options/strategies: 
The library comes with a bunch of options (internally referred to as strategies), for the following three main steps in the diffing process:

1. Filtering out irrelevant nodes and attributes
2. Matching up nodes and attributes for comparison
3. Comparing matched up nodes and attributes

The following section document the current built-in strategies that are available. A later second will describe how to built your own strategies, to get very tight control of the diffing process.

### Ignore comments
Enabling this strategy will ignore all comment nodes during comparison. Activate by calling the `IgnoreComments()` method on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .IgnoreComments()
    .Build();
```

_**NOTE**: Currently, the ignore comment strategy does NOT remove comments from CSS or JavaScript embedded in `<style>` or `<script>` tags._

### Whitespace handling
Whitespace can be a source of false-positives when comparing two HTML fragments. Thus, the whitespace handling strategy offer different ways to deal with it during a comparison.

- `Preserve`: Does not change or filter out any whitespace in control and test HTML. Default, same as not specifying any options.
- `RemoveWhitespaceNodes`: Using this option filters out all text nodes that only consist of whitespace characters.
- `Normalize`: Using this option will _trim_ all text nodes and replace two or more whitespace characters with a single space character.

These options can be set either _globally_ for the entire comparison, or on a _specific subtrees in the comparison_. 

To set a global default, call the method `Whitespace(WhitespaceOption)` on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .Whitespace(WhitespaceOption.Normalize)
    .Build();
```

To configure/override whitespace rules on a specific subtree in the comparison, use the `diff:whitespace="WhitespaceOption"` on a control node, and it and all nodes below it will use that whitespace option, unless it is overridden on a child node. In the example below, all whitespace inside the `<h1>` element is preserved:

```html
<header>
    <h1 diff:whitespace="Preserve">Hello   <em> woooorld</em></h1>
</header>
```

**Special case for `<pre>`-tags:** The content of `<pre />` tags will always be treated as the `Preserve` option, even if whitespace strategy is globally set to `RemoveWhitespaceNodes` or `Normalize`. To override this, add a local `diff:whitespace" attribute to the tag, e.g.:

```html
<pre diff:whitespace="RemoveWhitespaceNodes">...</pre>
```

**Special case for `<style>`-tags:** Even if the whitespace option is `Normalize`, whitespace inside quotes (`"` and `'` style quotes) is preserved as is. For example, the text inside the `content` style information in the following CSS will not be normalized: `p::after { content: " -.- "; }`.

**Special case for `<script>`-tags:**  It is on the issues list to deal with whitespace properly inside `<script>`-tags.

### Ignore attribute
If the `diff:ignore="true"` attribute is used on a control element  (`="true"` implicit/optional), all their attributes and child nodes are skipped/ignored during comparison, including those of the test element, the control element is matched with.

In this example, the `<h1>` tag, it's attribute and children are considered the same as the element it is matched with:

```html
<header>
    <h1 class="heading-1" diff:ignore>Hello world</h1>
</header>
```

To only ignore a specific attribute during comparison, add the `:ignore` to the attribute in the control HTML. That will consider the control and test attribute the same. E.g. to ignore the `class` attribute, do:

```html
<header>
    <h1 class:ignore="heading-1">Hello world</h1>
</header>
```

Activate this strategy by calling the `EnableIgnoreAttribute()` method on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .EnableIgnoreAttribute()
    .Build();
```

### Matching options

#### One-to-one matcher (node, attr)

#### Forward-searching matcher (node)

#### CSS selector-cross tree matcher (node, attr)

### Compare options
#### Name/Type comparer (node, attr)
#### Content comparer (text, attr)
#### Content regex comparer (text, attr)
#### IgnoreCase content comparer (text, attr)
#### Class attribute comparer (attr)
#### Boolean-attribute comparer (attr)
See rules at https://html.spec.whatwg.org/multipage/common-microsyntaxes.html#boolean-attributes
https://www.w3.org/TR/html52/infrastructure.html#sec-boolean-attributes
https://gist.github.com/ArjanSchouten/0b8574a6ad7f5065a5e7

### Ignoring special `diff:` attributes
Any attributes that starts with `diff:` are automatically filtered out before matching/comparing happens. E.g. `diff:whitespace="..."` does not show up as a missing diff when added to an control element.

To enable this option, use the `IgnoreDiffAttributes()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .IgnoreDiffAttributes()
    .Build();
```

## Difference engine details
The heart of the library is the `HtmlDifferenceEngine` class, which goes through the steps illustrated in the activity diagram below to determine if the control nodes is the same as the test nodes.

The `HtmlDifferenceEngine` class depends on three _strategies_, the `IFilterStrategy`, `IMatcherStrategy`, and `ICompareStrategy` types. These are used in the highlighted activities in the diagram. With those, we can control what nodes and attributes take part in the comparison (filter strategy), how control and test nodes and attributes are matched up for comparison (matching strategy), and finally, how nodes and attributes are determined to be same or different (compare strategy).

It starts with a call to the `Compare(INodeList controlNodes, INodeList testNodes)` and recursively calls itself when nodes have child nodes.

![img](docs/HtmlDifferenceEngineFlow.svg)

The library comes with a bunch of different filters, matchers, and conparers, that you can configure and mix and match with your own, to get the exact diffing experience you want. See the Usage section above for details.

## Creating custom diffing strategies

TODO!

### Filters
- default starting decision is `true`.
- if a filter receives a source that it does not have an opinion on, it should always return the current decision, whatever it may be.

## Acknowledgement
Big thanks to [Florian Rappl](https://github.com/FlorianRappl) from the AngleSharp team for providing ideas, input and sample code for working with AngleSharp. 

Another shout-out goes to [XMLUnit](https://www.xmlunit.org). It is a great XML diffing library, and it has been a great inspiration for creating this library.
