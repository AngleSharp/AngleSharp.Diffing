# AngleSharp.Diffing - HTML Comparison Library

AngleSharp.Diffing is a .NET library for comparing AngleSharp control nodes and test nodes to identify differences between HTML DOM trees. This library targets .NET Standard 2.0 and provides a fluent API for HTML comparison and diffing.

**Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Prerequisites and Setup
- Install .NET 8.0+ SDK (already available in GitHub Actions environments)
- Mono is NOT required for standard development (only for legacy Cake build system)

### Bootstrap, Build, and Test
Execute these commands in order from the repository root:

```bash
cd src
dotnet restore                 # ~11 seconds - restores NuGet packages
dotnet build                   # ~15 seconds - compiles library and tests  
dotnet test                    # ~5 seconds - runs 521 tests. NEVER CANCEL.
```

**CRITICAL TIMING EXPECTATIONS:**
- `dotnet restore`: 11 seconds - NEVER CANCEL. Set timeout to 60+ seconds minimum.
- `dotnet build`: 15 seconds - NEVER CANCEL. Set timeout to 60+ seconds minimum.
- `dotnet test`: 5 seconds - runs 521 tests. NEVER CANCEL. Set timeout to 30+ seconds minimum.
- Complete clean rebuild: ~20 seconds - NEVER CANCEL. Set timeout to 60+ seconds minimum.

### Alternative Build Methods
**PRIMARY METHOD (Recommended):** Use dotnet CLI commands as shown above.

**LEGACY METHOD (Not Recommended):** The repository includes Cake build scripts (`build.sh`/`build.ps1`) but they require Mono and may fail. Only use if specifically needed:
```bash
# Install Mono first (if needed)
sudo apt-get update && sudo apt-get install -y mono-complete
./build.sh  # May fail - use dotnet commands instead
```

### Release Builds and Packaging
```bash
cd src
dotnet build --configuration Release          # ~15 seconds
dotnet pack --configuration Release           # ~8 seconds - creates NuGet packages
```

**Package Output:** Creates `.nupkg` and `.snupkg` files in `src/AngleSharp.Diffing/bin/Release/`

## Validation and Testing

### Manual Validation Requirements
**ALWAYS** validate changes by creating and running a test program to exercise the library:

```csharp
using AngleSharp.Diffing;

// Test basic HTML diffing functionality
var control = @"<p attr=""foo"">hello <em>world</em></p>";
var test = @"<p attr=""bar"">hello <strong>world</strong></p>";

var diffs = DiffBuilder
    .Compare(control)
    .WithTest(test)
    .Build()
    .ToList();

Console.WriteLine($"Found {diffs.Count} differences:");
foreach (var diff in diffs)
{
    Console.WriteLine($"- {diff.GetType().Name}: {diff.Result} {diff.Target}");
}
```

### Code Quality and Standards
- **Code Analysis:** Project has strict analysis rules enabled (`EnforceCodeStyleInBuild=true`)
- **EditorConfig:** Follow the existing `.editorconfig` settings (4-space indentation, CRLF line endings)
- **Nullable References:** Enabled - handle null values appropriately
- **Build Warnings:** Zero warnings expected - treat warnings as errors for critical types

### Test Suite Information
- **Test Framework:** xUnit with Shouldly assertions
- **Test Count:** 521 tests covering core diffing functionality
- **Test Runtime:** ~5 seconds total
- **Test Files Location:** `src/AngleSharp.Diffing.Tests/`

## Navigation and Architecture

### Key Directories and Files
```
src/
├── AngleSharp.Diffing/              # Main library project
│   ├── Core/                        # Core diffing engine and interfaces
│   ├── Extensions/                  # Extension methods
│   ├── Strategies/                  # Diffing strategy implementations
│   └── DiffBuilder.cs              # Main fluent API entry point
├── AngleSharp.Diffing.Tests/        # Test project
│   ├── Core/                        # Core functionality tests
│   └── Strategies/                  # Strategy-specific tests
└── AngleSharp.Diffing.sln          # Solution file
```

### Important Classes and APIs
- **`DiffBuilder`**: Main entry point with fluent API (`DiffBuilder.Compare(control).WithTest(test).Build()`)
- **`IDiff`**: Base interface for all difference types
- **`HtmlDifferenceEngine`**: Core comparison engine
- **Diff Types**: `AttrDiff`, `NodeDiff`, `MissingNodeDiff`, `UnexpectedNodeDiff`, etc.

### Dependencies
- **AngleSharp 1.1.2**: Core HTML parsing library
- **AngleSharp.Css 1.0.0-beta.144**: CSS support
- **Target Framework**: .NET Standard 2.0 (library), .NET 8.0 (tests)

## Common Development Tasks

### Making Code Changes
1. **Always build and test first** to establish baseline: `dotnet build && dotnet test`
2. Make your changes to source files in `src/AngleSharp.Diffing/`
3. **Immediately test after changes**: `dotnet build && dotnet test`
4. **Manual validation**: Create a test program to exercise your changes
5. **Pre-commit validation**: Ensure no build warnings or test failures

### Debugging and Investigation
- **Test filtering**: `dotnet test --filter "TestMethodName"` to run specific tests
- **Verbose builds**: `dotnet build --verbosity normal` to see detailed output
- **Key test files**: `DiffBuilderTest.cs` shows basic API usage patterns

### CI/Build Requirements
- **GitHub Actions**: Runs on both Linux and Windows
- **Zero warnings policy**: Build must complete without warnings
- **All tests must pass**: 521/521 tests required
- **Code formatting**: Enforced via EditorConfig and analyzers

## Getting Started Examples

### Basic HTML Comparison
```csharp
var control = "<p>Expected content</p>";
var test = "<p>Actual content</p>";

var diffs = DiffBuilder
    .Compare(control)
    .WithTest(test)
    .Build();
```

### Advanced Configuration
```csharp
var diffs = DiffBuilder
    .Compare(control)
    .WithTest(test)
    .WithOptions(options => {
        // Configure diffing strategies
    })
    .Build();
```

### Working with Results
```csharp
foreach (var diff in diffs)
{
    switch (diff.Result)
    {
        case DiffResult.Different:
        case DiffResult.Missing:
        case DiffResult.Unexpected:
            // Handle different types of differences
            break;
    }
}
```

## Troubleshooting

### Common Issues
- **Build failures**: Ensure you're in the `src/` directory
- **Cake build failures**: Use `dotnet` commands instead of `build.sh`/`build.ps1`
- **Test failures**: Check for environment-specific issues or recent changes
- **Package restore issues**: Clear NuGet caches with `dotnet nuget locals all --clear`

### Quick Reset Commands
```bash
# Clean and rebuild everything
cd src
dotnet clean
rm -rf */bin */obj
dotnet restore
dotnet build
dotnet test
```

## Performance Expectations
- **Development builds**: ~15 seconds
- **Test execution**: ~5 seconds for full suite
- **Package creation**: ~8 seconds
- **CI builds**: Allow 60+ seconds timeout minimum for each step

**CRITICAL**: NEVER CANCEL builds or tests that appear slow. Wait at least 60 seconds for builds and 30 seconds for tests before considering alternatives.