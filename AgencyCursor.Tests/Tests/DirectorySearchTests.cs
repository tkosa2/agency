using System.IO;
using Xunit;

namespace AgencyCursor.Tests.Tests;

public class DirectorySearchTests
{
    private static string WebAppRoot =>
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "AgencyCursor.WebApp"));

    [Fact]
    public void DirectoryDbExists()
    {
        var dbPath = Path.Combine(
            Path.GetFullPath(Path.Combine(WebAppRoot, "..")),
            "directory",
            "rid_interpreters.db");
        Assert.True(File.Exists(dbPath), $"Expected directory DB at {dbPath}");
    }

    [Fact]
    public void SearchDirectoryPageExists()
    {
        var cshtml = Path.Combine(WebAppRoot, "Pages", "Interpreters", "SearchDirectory.cshtml");
        var cs = Path.Combine(WebAppRoot, "Pages", "Interpreters", "SearchDirectory.cshtml.cs");
        Assert.True(File.Exists(cshtml), "SearchDirectory.cshtml should exist");
        Assert.True(File.Exists(cs), "SearchDirectory.cshtml.cs should exist");
    }

    [Fact]
    public void RegisterFromDirectoryPageExists()
    {
        var cshtml = Path.Combine(WebAppRoot, "Pages", "Interpreters", "RegisterFromDirectory.cshtml");
        var cs = Path.Combine(WebAppRoot, "Pages", "Interpreters", "RegisterFromDirectory.cshtml.cs");
        Assert.True(File.Exists(cshtml), "RegisterFromDirectory.cshtml should exist");
        Assert.True(File.Exists(cs), "RegisterFromDirectory.cshtml.cs should exist");
    }

    [Fact]
    public void SearchDirectoryPageModelExists()
    {
        var type = typeof(AgencyCursor.WebApp.Pages.Interpreters.SearchDirectoryModel);
        Assert.NotNull(type);
    }

    [Fact]
    public void RegisterFromDirectoryPageModelExists()
    {
        var type = typeof(AgencyCursor.WebApp.Pages.Interpreters.RegisterFromDirectoryModel);
        Assert.NotNull(type);
    }

    [Fact]
    public void DirectorySearchServiceInterfaceExists()
    {
        var type = typeof(AgencyCursor.WebApp.Services.IDirectorySearchService);
        Assert.NotNull(type);
    }

    [Fact]
    public void DirectorySearchServiceHasSearchMethod()
    {
        var type = typeof(AgencyCursor.WebApp.Services.IDirectorySearchService);
        var method = type.GetMethod("SearchAsync");
        Assert.NotNull(method);
        // Verify all required filter parameters are present
        var paramNames = method!.GetParameters().Select(p => p.Name).ToArray();
        Assert.Contains("firstName", paramNames);
        Assert.Contains("lastName", paramNames);
        Assert.Contains("certificate", paramNames);
        Assert.Contains("specialty", paramNames);
        Assert.Contains("city", paramNames);
        Assert.Contains("state", paramNames);
        Assert.Contains("zip", paramNames);
    }

    [Fact]
    public void InterpretersIndexContainsSearchDirectoryButton()
    {
        var content = File.ReadAllText(Path.Combine(WebAppRoot, "Pages", "Interpreters", "Index.cshtml"));
        Assert.Contains("SearchDirectory", content);
        Assert.Contains("Search Directory", content);
    }

    [Fact]
    public void LayoutContainsSearchDirectoryNavLink()
    {
        var content = File.ReadAllText(Path.Combine(WebAppRoot, "Pages", "Shared", "_Layout.cshtml"));
        Assert.Contains("SearchDirectory", content);
    }
}
