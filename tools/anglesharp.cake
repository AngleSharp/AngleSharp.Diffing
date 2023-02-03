#addin nuget:?package=Cake.FileHelpers&version=4.0.1
#addin nuget:?package=Octokit&version=0.32.0
using Octokit;

var configuration = Argument("configuration", "Release");
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var isRunningOnGitHubActions = BuildSystem.GitHubActions.IsRunningOnGitHubActions;
var releaseNotes = ParseReleaseNotes("./CHANGELOG.md");
var version = releaseNotes.RawVersionLine.Substring(2);
var buildDir = Directory($"./src/{projectName}/bin") + Directory(configuration);
var buildResultDir = Directory("./bin") + Directory(version);
var nugetRoot = buildResultDir + Directory("nuget");

if (isRunningOnGitHubActions)
{
    var buildNumber = BuildSystem.GitHubActions.Environment.Workflow.RunNumber;

    if (target == "Default")
    {
        version = $"{version}-ci-{buildNumber}";
    }
    else if (target == "PrePublish")
    {
        version = $"{version}-alpha-{buildNumber}";
    }
}

if (!isRunningOnWindows)
{
    frameworks.Remove("net46");
    frameworks.Remove("net461");
    frameworks.Remove("net472");
}

// Initialization
// ----------------------------------------

Setup(_ =>
{
    Information($"Building version {version} of {projectName}.");
    Information("For the publish target the following environment variables need to be set:");
    Information("- NUGET_API_KEY");
    Information("- GITHUB_API_TOKEN");
});

// Tasks
// ----------------------------------------

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(new DirectoryPath[] { buildDir, buildResultDir, nugetRoot });
    });

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore($"./src/{solutionName}.sln", new NuGetRestoreSettings
        {
            ToolPath = "tools/nuget.exe",
        });
    });

Task("Build")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
    {
        var buildSettings = new DotNetCoreMSBuildSettings();
        buildSettings.WarningCodesAsMessage.Add("CS1591"); // During CI builds we don't want the output polluted by "warning CS1591: Missing XML comment" warnings

        ReplaceRegexInFiles("./src/Directory.Build.props", "(?<=<Version>)(.+?)(?=</Version>)", version);
        DotNetCoreBuild($"./src/{solutionName}.sln", new DotNetCoreBuildSettings
        {
           Configuration = configuration,
           MSBuildSettings = buildSettings
        });
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoBuild = false,
        };

        if (isRunningOnGitHubActions)
        {
            settings.TestAdapterPath = Directory(".");
            settings.Loggers.Add("GitHubActions");
        }

        DotNetCoreTest($"./src/{solutionName}.Tests/", settings);
    });

Task("Copy-Files")
    .IsDependentOn("Build")
    .Does(() =>
    {
        foreach (var item in frameworks)
        {
            var targetDir = nugetRoot + Directory("lib") + Directory(item.Key);
            CreateDirectory(targetDir);
            CopyFiles(new FilePath[]
            {
                buildDir + Directory(item.Value) + File($"{projectName}.dll"),
                buildDir + Directory(item.Value) + File($"{projectName}.xml"),
            }, targetDir);
        }

        CopyFiles(new FilePath[] {
            $"src/{projectName}.nuspec",
            "logo.png"
        }, nugetRoot);
    });

Task("Create-Package")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var settings = new DotNetCorePackSettings
        {
            Configuration = configuration,
		    NoBuild = false,
            OutputDirectory =  nugetRoot,
		    ArgumentCustomization = args => args.Append("--include-symbols").Append("-p:SymbolPackageFormat=snupkg")
        };

        DotNetCorePack($"./src/{solutionName}/", settings);
    });

Task("Publish-Package")
    .IsDependentOn("Create-Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
    {
        var apiKey = EnvironmentVariable("NUGET_API_KEY");

        if (String.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Could not resolve the NuGet API key.");
        }

        foreach (var nupkg in GetFiles(nugetRoot.Path.FullPath + "/*.nupkg"))
        {
            NuGetPush(nupkg, new NuGetPushSettings
            {
                Source = "https://nuget.org/api/v2/package",
                ApiKey = apiKey,
				SkipDuplicate = true
            });
        }
    });

Task("Publish-Release")
    .IsDependentOn("Publish-Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
    {
        var githubToken = EnvironmentVariable("GITHUB_TOKEN");

        if (String.IsNullOrEmpty(githubToken))
        {
            throw new InvalidOperationException("Could not resolve GitHub token.");
        }

        var github = new GitHubClient(new ProductHeaderValue("AngleSharpCakeBuild"))
        {
            Credentials = new Credentials(githubToken),
        };

        var newRelease = github.Repository.Release;
        newRelease.Create("AngleSharp", projectName, new NewRelease("v" + version)
        {
            Name = version,
            Body = String.Join(Environment.NewLine, releaseNotes.Notes),
            Prerelease = releaseNotes.Version.ToString() != version,
            TargetCommitish = "master",
        }).Wait();
    });

// Targets
// ----------------------------------------

Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Create-Package");

Task("Default")
    .IsDependentOn("Package");

Task("Publish")
    .IsDependentOn("Publish-Release");

Task("PrePublish")
    .IsDependentOn("Publish-Package");
