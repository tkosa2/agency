using System.IO;
using Xunit;

namespace AgencyCursor.Tests.Tests;

public class DarkModeTests
{
    private static string WebAppRoot =>
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "AgencyCursor.WebApp"));

    [Fact]
    public void SiteJsContainsDarkModeToggleFunction()
    {
        var content = File.ReadAllText(Path.Combine(WebAppRoot, "wwwroot", "js", "site.js"));
        Assert.Contains("toggleDarkMode", content);
        Assert.Contains("dark-mode", content);
        Assert.Contains("localStorage", content);
    }

    [Fact]
    public void SiteCssContainsDarkModeStyles()
    {
        var content = File.ReadAllText(Path.Combine(WebAppRoot, "wwwroot", "css", "site.css"));
        Assert.Contains("body.dark-mode", content);
        Assert.Contains("#121212", content);
    }

    [Fact]
    public void LayoutContainsDarkModeToggleButton()
    {
        var content = File.ReadAllText(Path.Combine(WebAppRoot, "Pages", "Shared", "_Layout.cshtml"));
        Assert.Contains("darkModeToggle", content);
        Assert.Contains("site.js", content);
    }
}
