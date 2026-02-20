using Xunit;

namespace AgencyCursor.Tests.Tests;

public class HomePageTests
{
    [Fact]
    public void HomePageModelExists()
    {
        // Basic compilation test - verifies the app assembles correctly
        var type = typeof(AgencyCursor.WebApp.Pages.IndexModel);
        Assert.NotNull(type);
    }
}
