# 0.17.0

Released on Wednesday, September 8, 2021.

- Added the ability to ignore an elements children or its attributes. By [@grishat](https://github.com/grishat).

# 0.16.0

Released on Wednesday, June 24, 2021.

- Upgraded to version 0.16.0 of AngleSharp.

# 0.15.0

Released on Wednesday, April 28, 2021.

- Upgraded to version 0.15.0 of AngleSharp.
- Added strong name signing of assembly.

# 0.14.0

Released on Wednesday, April 16, 2020.

- Bug: custom whitespace options on `<pre>`/`<style>`/`<script>` not being applied during comparison.
- Upgraded to version 0.14.0 of AngleSharp.

# 0.13.2

Released on Thursday, December 27, 2019.

- Added code documentation to all public methods.
- Refactored IDiffingStrategyCollection's methods to take a `StrategyType strategyType = StrategyType.Specialized` as input instead of `bool isSpecialized* = true` argument.
- Fixed bug where `TextNodeFilter` would not give `<style>` and `<script>` the `WhitespaceOption.Preserve` by default.
- Fixed bug where `IgnoreElementComparer` would not change a current decision to SKip if it was Same.

# 0.13.1

Released on Thursday, December 26, 2019.

Small point release with an additional method added to the `HtmlDiffer` class.

# 0.13.0

Released on Wednesday, December 25, 2019.

- Updated path's index' calculation in `ComparisonSource` to only include nodes that implement the IParentNode.
- Small change to HtmlDifferenceEngine. It now takes the control and test sources during construction. This makes it clear it is single use. For reusable differ, use `HtmlDiffer` going forward.
- Added interface `IDiffContext` and made `DiffContext` internal.

# 0.13.0-preview-3

Released on Sunday, November 24, 2019.

- Added `Compare(INode controlNode, INode testNode)` to HtmlDifferenceEngine
- Changed existing `Compare` method in HtmlDifferenceEngine to take `IEnumerable<INode>` instead of `INodeList`.

# 0.13.0-preview-2

Released on Sunday, November 3, 2019.

- Fixed error in repository url reported to nuget.

# 0.13.0-preview-1

Released on Sunday, November 3, 2019.

This is the initial preview release of AngleSharp.Diffing. 
