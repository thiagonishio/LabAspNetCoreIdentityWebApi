#tool "nuget:?package=GitVersion.CommandLine"
#addin nuget:?package=Newtonsoft.Json

using Newtonsoft.Json;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Development");

Task("Build")
.Does(() =>
{
    foreach(var project in GetFiles("./src/**/*.csproj"))
    {
        DotNetCoreBuild(
            project.GetDirectory().FullPath, 
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration
            });
    }
});

Task("Test")
.IsDependentOn("Build")
.Does(() =>
{
    foreach(var project in GetFiles("./tests/**/*.csproj"))
    {
        DotNetCoreTest(
            project.GetDirectory().FullPath,
            new DotNetCoreTestSettings()
            {
                Configuration = configuration
            });
    }
});

Task("Default")
.IsDependentOn("Test");

RunTarget(target);

private bool ShouldRunRelease() => AppVeyor.IsRunningOnAppVeyor && AppVeyor.Environment.Repository.Tag.IsTag;

private string GetPackageVersion()
{
    var gitVersion = GitVersion(new GitVersionSettings {
        RepositoryPath = "."
    });

    Information($"Git Semantic Version: {JsonConvert.SerializeObject(gitVersion)}");
    
    return gitVersion.NuGetVersionV2;
}