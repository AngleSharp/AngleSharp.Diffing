#!/usr/bin/env bash
###############################################################
# This is a modern .NET build script that replaces the legacy
# Cake-based build system to avoid mono dependency on Linux.
###############################################################

set -eo pipefail

# Define directories.
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
SRC_DIR=$SCRIPT_DIR/src
SOLUTION_FILE=$SRC_DIR/AngleSharp.Diffing.sln

# Define default arguments.
TARGET="Default"
CONFIGURATION="Release"
VERBOSITY="minimal"
SHOW_VERSION=false

# Parse arguments.
while [[ $# -gt 0 ]]; do
    case $1 in
        -t|--target) TARGET="$2"; shift 2 ;;
        -c|--configuration) CONFIGURATION="$2"; shift 2 ;;
        -v|--verbosity) VERBOSITY="$2"; shift 2 ;;
        --version) SHOW_VERSION=true; shift ;;
        --) shift; break ;;
        *) shift ;;
    esac
done

# Show dotnet version if requested
if $SHOW_VERSION; then
    dotnet --version
    exit 0
fi

echo "Building AngleSharp.Diffing with target '$TARGET' and configuration '$CONFIGURATION'"

# Ensure we have dotnet CLI available
if ! command -v dotnet &> /dev/null; then
    echo "Error: dotnet CLI is not installed or not in PATH"
    exit 1
fi

echo "Using .NET SDK version: $(dotnet --version)"

# Change to source directory
cd $SRC_DIR

# Build based on target
case $TARGET in
    "Clean")
        echo "Cleaning build artifacts..."
        dotnet clean $SOLUTION_FILE --configuration $CONFIGURATION --verbosity $VERBOSITY
        ;;
    "Restore"|"Restore-Packages")
        echo "Restoring NuGet packages..."
        dotnet restore $SOLUTION_FILE --verbosity $VERBOSITY
        ;;
    "Build")
        echo "Building solution..."
        dotnet build $SOLUTION_FILE --configuration $CONFIGURATION --verbosity $VERBOSITY
        ;;
    "Test"|"Run-Unit-Tests")
        echo "Running tests..."
        dotnet test $SOLUTION_FILE --configuration $CONFIGURATION --verbosity $VERBOSITY
        ;;
    "Package"|"Create-Package")
        echo "Building and creating packages..."
        dotnet build $SOLUTION_FILE --configuration $CONFIGURATION --verbosity $VERBOSITY
        dotnet pack $SOLUTION_FILE --configuration $CONFIGURATION --no-build --verbosity $VERBOSITY
        ;;
    "Publish"|"Publish-Package")
        echo "Building, testing, and publishing packages..."
        dotnet build $SOLUTION_FILE --configuration $CONFIGURATION --verbosity $VERBOSITY
        dotnet test $SOLUTION_FILE --configuration $CONFIGURATION --no-build --verbosity $VERBOSITY
        dotnet pack $SOLUTION_FILE --configuration $CONFIGURATION --no-build --verbosity $VERBOSITY
        
        # Publish to NuGet if API key is available
        if [ ! -z "$NUGET_API_KEY" ]; then
            echo "Publishing packages to NuGet..."
            for nupkg in $(find . -name "*.nupkg" -not -path "*/bin/Debug/*"); do
                echo "Publishing $nupkg"
                dotnet nuget push "$nupkg" --api-key "$NUGET_API_KEY" --source https://api.nuget.org/v3/index.json --skip-duplicate
            done
        else
            echo "NUGET_API_KEY not set, skipping NuGet publish"
        fi
        ;;
    "Default"|*)
        echo "Running default build (build + test + package)..."
        dotnet build $SOLUTION_FILE --configuration $CONFIGURATION --verbosity $VERBOSITY
        dotnet test $SOLUTION_FILE --configuration $CONFIGURATION --no-build --verbosity $VERBOSITY
        dotnet pack $SOLUTION_FILE --configuration $CONFIGURATION --no-build --verbosity $VERBOSITY
        ;;
esac

echo "Build completed successfully!"