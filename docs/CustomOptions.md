# Creating custom diffing strategies

### Filters
- default starting decision is `true`.
- if a filter receives a source that it does not have an opinion on, it should always return the current decision, whatever it may be.