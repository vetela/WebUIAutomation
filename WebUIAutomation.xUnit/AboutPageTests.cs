using FluentAssertions;
using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Pages;
using static WebUIAutomation.Shared.Logging;

namespace WebUIAutomation.xUnit;

public class AboutPageTests : IClassFixture<WebDriverFixture>
{
	private readonly HomePage _homePage;
	public AboutPageTests(WebDriverFixture fixture) => _homePage = new HomePage();

	[Fact]
	[Trait("Category", "Navigation")]
	public void VerifyAboutEHUPageLoadsCorrectly()
	{
		var testName = nameof(VerifyAboutEHUPageLoadsCorrectly);
		Logger.Information("Starting test: {TestName}", testName);

		try
		{
			_homePage.NavigateTo();
			Logger.Debug("Navigated to Home Page.");

			var aboutPage = _homePage.GoToAboutPage();
			Logger.Debug("Navigated to About Page.");

			WebDriverSingleton.Instance.Driver.Url.Should().Be("https://en.ehu.lt/about/");
			Logger.Information("Verified URL matches expected.");

			aboutPage.GetTitle().Should().Be("About");
			Logger.Information("Verified page title matches expected.");

			aboutPage.GetHeaderText().Should().Be("About");
			Logger.Information("Verified page header matches expected.");
		}
		catch (Exception ex)
		{
			Logger.Error(ex, "An error occurred in test: {TestName}", testName);
			throw;
		}
		finally
		{
			Logger.Information("Test {TestName} completed.", testName);
		}
	}
}
