# Diffing options/strategies: 
The library comes with a bunch of options (internally referred to as strategies), for the following three main steps in the diffing process:

1. Filtering out irrelevant nodes and attributes
2. Matching up nodes and attributes for comparison
3. Comparing matched up nodes and attributes

To make it easier to configure the diffing engine, the library comes with a `DiffBuilder` class, which handles the relative complex task of setting up the `HtmlDifferenceEngine`.

Using the `UseDefaultOptions()` method is equivalent to setting the following options explicitly:

To learn how to create your own strategies, visit the [Custom Strategies](CustomStrategies.md) page.

The following section documents the current built-in strategies that are available. 

## Default Options
In most cases, calling the `UseDefaultOptions()` method on a `DiffBuilder` instance will give you a good set of strategies for a comparison, e.g.

```csharp
var controlHtml = "<p>Hello World</p>";
var testHtml = "<p>World, I say hello</p>";
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithDefaultOptions()
    .Build();
```

Calling the `UseDefaultOptions()` method is equivalent to specifying the following options explicitly: 

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .IgnoreDiffAttributes()
    .IgnoreComments()
    .WithSearchingNodeMatcher()
    .WithCssSelectorMatcher()
    .WithAttributeNameMatcher()
    .WithNodeNameComparer()
    .WithIgnoreElementSupport()
    .WithStyleSheetComparer()
    .WithTextComparer(WhitespaceOption.Normalize, ignoreCase: false)
    .WithAttributeComparer()
    .WithClassAttributeComparer()
    .WithBooleanAttributeComparer(BooleanAttributeComparision.Strict)
    .WithInlineAttributeIgnoreSupport()
    .Build();
``` 

Read more about each of the strategies below, including some that are not part of the default set.

## Filter strategies
These are the built-in filter strategies.

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

### Ignoring special "diff"-attributes
Any attributes that starts with `diff:` are automatically filtered out before matching/comparing happens. E.g. `diff:whitespace="..."` does not show up as a missing diff when added to an control element. 

To enable this option, use the `IgnoreDiffAttributes()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .IgnoreDiffAttributes()
    .Build();
```

## Matching strategies
These are the built-in matching strategies. We have two different types, one for nodes and one for attributes.

### Node matchers (elements, text, comments, etc.)
These are the built-in node matching strategies. They cover elements, text nodes, comments, and other types that inherit from `INode`.

#### One-to-one node matcher
The one-to-one node-matching strategy simply matches two node lists with each other, based on each nodes index. So, if you have two equal length control and test node lists, `controlNodes[0]` will be matched with `testNodes[0]`, `controlNodes[1]` with `testNodes[1]`, and so on.

If either of the lists is shorter than the other, the remaining items will be reported as *missing* (for control nodes) or *unexpected* (for test nodes).

If a node has been marked as matched by a previous executed matcher, the One-to-one matcher will not use that node in its matching, and skip over it.

To choose this matcher, use the `WithOneToOneNodeMatcher()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithOneToOneNodeMatcher()
    .Build();
```

#### Forward-searching node matcher
The forward-searching node-matcher strategy will only match control nodes with test nodes, if their `NodeName` match. It does this by taking one control node at the time, and searching after the previously matched test node until it finds a match. If it does not, continues with the next control node, and the unmatched control node is marked as *missing*. After, any unmatched test nodes is marked as *unexpected*. 

The follow JavaScript-ish-code  illustrates how the algorithm works:

```js
forwardSearchingMatcher(controlNodes, testNodes) {
    let matches = []
    let lastMatchedTestNode = -1
    
    foreach(controlNode in controlNodes) {
        var index = lastMatchedTestNode + 1

        while(index < testNodes.length) {
            if(controlNode.NodeName == testNodes[index].NodeName) {
                matches.push((controlNode, testNodes[index]))
                lastMatchedTestNode = index
                index = testNodes.length
            }
            index++
        }
    }
    return matches
}
```

To choose this matcher, use the `WithSearchingNodeMatcher()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithSearchingNodeMatcher()
    .Build();
```

#### CSS-selector element matcher
The CSS-selector matcher can be used to match any test element from the test node tree with a given control element. On the control element, add the `diff:match="CSS selector"` attribute. The specified `CSS selector` should only match a _zero_ or _one_ test element. 

For example, if the _test nodes_ looks like this:

```html
<header>
    <h1>hello world</h1>
</header>
<main>
...
</main>
<footer>
...
</footer>
```

The following control node will be compared against the `<h1>` in the `<header>` tag:

```html
<h1 diff:match="header > h1">hello world</h1>
```

One use case of the CSS-selector element matcher is where you only want to test one part of a sub-tree, and ignore the rest. The example above will report the unmatched test nodes as *unexpected*, but those "diffs" can be ignored, since that is expected. This approach can save you from specifying all the needed control nodes, if only part of a sub tree needs to be compared.

To choose this matcher, use the `WithCssSelectorMatcher()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithCssSelectorMatcher()
    .Build();
```

### Attribute matching strategies
These are the built-in attribute matching strategies.

#### Attribute name matcher
This selector will match attributes on a control element with attributes on a test element using the attribute's name. If an *control* attribute is not matched, it is reported as *missing* and if a *test* attribute is not matched, it is reported as *unexpected*.

To choose this matcher, use the `WithAttributeNameMatcher()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithAttributeNameMatcher()
    .Build();
```

## Comparing strategies
These are the built-in comparing strategies.

### Node and element compare strategy
The basic node compare strategy will simply check if the node's types and node's name are equal.

To choose this comparer, use the `WithNodeNameComparer()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithNodeNameComparer()
    .Build();
```

#### Ignore element attribute
If the `diff:ignore="true"` attribute is used on a control element (`="true"` implicit/optional), all their attributes and child nodes are skipped/ignored during comparison, including those of the test element, the control element is matched with.

In this example, the `<h1>` tag, it's attribute and children are considered the same as the element it is matched with:

```html
<header>
    <h1 class="heading-1" diff:ignore>Hello world</h1>
</header>
```

Activate this strategy by calling the `WithIgnoreElementSupport()` method on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithIgnoreElementSupport()
    .Build();
```

### Text (text nodes) strategies
The built-in text strategies offer a bunch of ways to control how text (text nodes) is handled during the diffing process.

**NOTE:** It is on the issues list to enable a more intelligent, e.g. whitespace aware, comparison of JavaScript (text) inside `<script>`-tags and event-attributes.

#### Whitespace handling
Whitespace can be a source of false-positives when comparing two HTML fragments. Thus, the whitespace handling strategy offer different ways to deal with it during a comparison.

- `Preserve` (default): Does not change or filter out any whitespace in text nodes the control and test HTML.
- `RemoveWhitespaceNodes`: Using this option filters out all text nodes that only consist of whitespace characters.
- `Normalize`: Using this option will _trim_ all text nodes and replace two or more whitespace characters with a single space character. This option implicitly includes the `RemoveWhitespaceNodes` option.

These options can be set either _globally_ for the entire comparison, or inline on a _specific subtrees in the comparison_. 

To set a global default, call the method `WithTextComparer(WhitespaceOption)` on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithTextComparer(WhitespaceOption.Normalize)
    .Build();
```

To configure/override whitespace rules on a specific subtree in the comparison, use the `diff:whitespace="WhitespaceOption"` inline on a control element, and it and all text nodes below it will use that whitespace option, unless it is overridden on a child element. In the example below, all whitespace inside the `<h1>` element is preserved:

```html
<header>
    <h1 diff:whitespace="preserve">Hello   <em> woooorld</em></h1>
</header>
```

**Special case for `<pre>` elements:** The content of `<pre>` elements will always be treated as the `Preserve` option, even if whitespace option is globally set to `RemoveWhitespaceNodes` or `Normalize`. To override this, add a in-line `diff:whitespace` attribute to the `<pre>`-tag, e.g.:

```html
<pre diff:whitespace="RemoveWhitespaceNodes">...</pre>
```

#### Perform case-_insensitve_ text comparison
To compare the text in two text nodes to each other using a case-insensitive comparison, call the `WithTextComparer(ignoreCase: true)` method on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithTextComparer(ignoreCase: true)
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

#### Style sheet text comparer
Different rules whitespace apply to style sheets (style information) inside `<style>` tags and `style="..."` attributes, than to HTML5. This comparer will parse the style information inside `<style>` tags and `style="..."` attributes and compare the result of the parsing, instead doing a direct string comparison. This should remove false-positives where e.g. insignificant whitespace makes two otherwise equal set of style informations result in a diff.

To add this comparer, use the `WithStyleSheetComparer()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithStyleSheetComparer()
    .Build();
```

### Attribute Compare options
The library supports various ways to perform attribute comparison. 

#### Basic name and value comparison
The *"name and value comparison"* is the base comparison option, and that will test if both the names and the values of the control and test attributes are equal. E.g.:

- `attr="foo"` is the same as `attr="foo"`
- `attr="foo"` is the NOT same as `attr="bar"`
- `foo="attr"` is the NOT same as `bar="attr"`

To choose this comparer, use the `WithAttributeComparer()` method on the `DiffBuilder` class, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithAttributeComparer()
    .Build();
```

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

### Ignore attributes during diffing
To ignore a specific attribute during comparison, add the `:ignore` postfix to the attribute on the control element. Thus will simply skip comparing the two attributes and not report any differences between them. E.g. to ignore the `class` attribute, do:

```html
<header>
    <h1 class:ignore="heading-1">Hello world</h1>
</header>
```

Activate this strategy by calling the `WithInlineAttributeIgnoreSupport()` method on a `DiffBuilder` instance, e.g.:

```csharp
var diffs = DiffBuilder
    .Compare(controlHtml)
    .WithTest(testHtml)
    .WithInlineAttributeIgnoreSupport()
    .Build();
```