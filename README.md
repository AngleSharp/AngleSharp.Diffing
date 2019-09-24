Steps in finding differences:

- NodeSelector/NodeFilter - filters away nodes that should not be part of the comparison
  - Inline filter(ignorer)
- NodeMatcher - matches a control-node with a test-node for comparison
- Compare nodes by:
  - AttributeFilter - filters away attributes not compared
    - Inline filter(ignorer)
  - AttributeMatcher - matches control-attr with test-attr for comparison
    - Inline matcher(css selector)
  - For each attribute-compare set:
    - apply attribute compare function
    - apply inline-attribute-compare function
  - For each child-node - compare recursively between control and test child-nodes
