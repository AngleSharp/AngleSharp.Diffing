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

### Text (text nodes) strategies
The built-in text strategies offer a bunch of ways to control how text (text nodes) is handled during the diffing process.

#### Whitespace handling
Whitespace can be a source of false-positives when comparing two HTML fragments. Thus, the whitespace handling strategy offer different ways to deal with it during a comparison.

- `Preserve` (default): Does not change or filter out any whitespace in text nodes the control and test HTML.
- `RemoveWhitespaceNodes`: Using this option filters out all text nodes that only consist of whitespace characters.
- `Normalize`: Using this option will _trim_ all text nodes and replace two or more whitespace characters with a single space character. This option implicitly includes the `RemoveWhitespaceNodes` option.

These options can be set either _globally_ for the entire comparison, or inline on a _specific subtrees in the comparison_. 

To set a global default, call the method `Whitespace(WhitespaceOption)` on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .Whitespace(WhitespaceOption.Normalize)
    .Build();
```

To configure/override whitespace rules on a specific subtree in the comparison, use the `diff:whitespace="WhitespaceOption"` inline on a control element, and it and all text nodes below it will use that whitespace option, unless it is overridden on a child element. In the example below, all whitespace inside the `<h1>` element is preserved:

```html
<header>
    <h1 diff:whitespace="preserve">Hello   <em> woooorld</em></h1>
</header>
```

**Special case for `<pre>` elements:** The content of `<pre>` elements will always be treated as the `Preserve` option, even if whitespace option is globally set to `RemoveWhitespaceNodes` or `Normalize`. To override this, add a inline `diff:whitespace` attribute to the `<pre>`-tag, e.g.:

```html
<pre diff:whitespace="RemoveWhitespaceNodes">...</pre>
```

**NOTE:** It is on the issues list to deal with whitespace properly inside `<style>` and `<script>`-tags, e.g. inside strings.

#### Perform case-_insensitve_ text comparison
To compare the text in two text nodes to eachother using a case-insensitive comparison, call the `IgnoreCase()` method on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .IgnoreCase()
    .Build();
```

To configure/override ignore case rules on a specific subtree in the comparison, use the `diff:ignoreCase="true|false"` inline on a control element, and it and all text nodes below it will use that ignore case setting, unless it is overridden on a child element. In the example below, ignore case is set active for all text inside the `<h1>` element:

```html
<header>
    <h1 diff:ignoreCase="true">Hello   <em> woooorld</em></h1>
</header>
```

Note, as with all HTML5 boolean attributes, the `="true"` or `="false"` parts are optional.

#### Use regular expression when comparing text
By using the inline attribute `diff:regex` on the element containing the text node being compared, the comparer will consider the control text to be a regular expression, and will use that to test whether the test text node is as expected. This can be combined with the inline `diff:ignoreCase` attribute, to make the regular expression case-insensitive. E.g.:

```html
<header>
    <h1 diff:regex diff:ignoreCase>Hello World \d{4}</h1>
</header>
```

The above  control text would use case-insensitive regular expression to match against a test text string (e.g. "HELLO WORLD 2020").

### Inline Ignore attribute
If the inline `diff:ignore="true"` attribute is used on a control element  (`="true"` implicit/optional), all their attributes and child nodes are skipped/ignored during comparison, including those of the test element, the control element is matched with.

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
    .EnableInlineIgnore()
    .Build();
```

### Attr Compare options
The library supports various ways to perform attribute comparison. 

#### Strict name and value comparison
The *"name and value comparison"* is the base comparison option, and that will test if both the names and the values of the control and test attributes are equal. E.g.:

- `attr="foo"` is the same as `attr="foo"`
- `attr="foo"` is the NOT same as `attr="bar"`
- `foo="attr"` is the NOT same as `bar="attr"`

This comparison mode is on by default.

#### RegEx attribute value comparer
It is possible to specify a regular expression in the control attributes value, and add the `:regex` postfix to the *control* attributes name, to have the comparison performed using a Regex match test. E.g. 

- `attr:regex="foo-\d{4}"` is the same as `attr="foo-2019"`

#### Ignore case attribute value comparer
To get the comparer to perform a case insensitive comparison of the values of the control and test attribute, add the `:ignoreCase` postfix to the *control* attributes name. E.g.

- `attr:ignoreCase="FOO"` is the same as `attr="foo"`

#### Combine ignore case and regex attribute value comparer
To perform a case insenstive regular expression match, combine `:ignoreCase` and `:regex` as a postfix to the *control* attributes name. The order you combine them does not matter. E.g. 

- `attr:ignoreCase:regex="FOO-\d{4}"` is the same as `attr="foo-2019"`
- `attr:regex:ignoreCase="FOO-\d{4}"` is the same as `attr="foo-2019"`

#### Class attribute comparer
The class attribute is special in HTML. It can contain a space separated list of CSS classes, whoes order does not matter. Therefor the library will ignore the order the CSS classes is specified in the class attribute of the control and test elements, and instead just ensure that both have the same CSS classes added to it. E.g.

- `class="foo bar"` is the same as `class="bar foo"`

To enable the special handling of the class attribute, call the `WithClassAttributeComparer()` on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithClassAttributeComparer()
    .Build();
```

#### Boolean attributes comparer
Another special type of attributes are the [boolean attributes](https://www.w3.org/TR/html52/infrastructure.html#sec-boolean-attributes). To make comparing these more forgiving, the boolean attribute comparer will consider two boolean attributes equal, according to these rules:

- In **strict** mode, a boolean attribute's value is considered truthy if the value is missing, empty, or is the name of the attribute.
- In **loose** mode, a boolean attribute's value is considered truthy if the attribute is present on an element.

For example, in **strict** mode, the following are considered equal:

- `required` is the same as `required=""`
- `required=""` is the same as `required="required"`
- `required="required"` is the same as `required="required"`

To enable the special handling of boolean attributes, call the `WithBooleanAttributeComparer(BooleanAttributeComparision.Strict)` or `WithBooleanAttributeComparer(BooleanAttributeComparision.Loose)` on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithBooleanAttributeComparer(BooleanAttributeComparision.Strict)
    .Build();
```

### Matching options

#### One-to-one matcher (node, attr)

#### Forward-searching matcher (node)

#### CSS selector-cross tree matcher (node, attr)

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
