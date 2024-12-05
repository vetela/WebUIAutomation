using FluentAssertions;
using OpenQA.Selenium;
using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Pages;
using static WebUIAutomation.Shared.Logging;

namespace WebUIAutomation.xUnit;


public class LanguageChangeTests : IClassFixture<WebDriverFixture>
{
	private readonly HomePage _homePage;

	public LanguageChangeTests(WebDriverFixture fixture)
	{
		_homePage = new HomePage();
	}

	[Fact]
	[Trait("Category", "Language")]
	public void VerifyLanguageChangeToLithuanian()
	{
		var testName = nameof(VerifyLanguageChangeToLithuanian);
		Logger.Information("Starting test: {TestName}", testName);

		try
		{
			_homePage.NavigateTo();
			Logger.Debug("Navigated to Home Page.");

			_homePage.SwitchToLithuanianLanguage();
			Logger.Debug("Switched language to Lithuanian.");

			WebDriverSingleton.Instance.Driver.Url.Should().Be("https://lt.ehu.lt/", "The URL does not indicate the language has switched to Lithuanian.");
			Logger.Information("Verified URL matches expected Lithuanian URL.");

			var htmlTag = WebDriverSingleton.Instance.Driver.FindElement(By.TagName("html"));
			var langAttribute = htmlTag.GetAttribute("lang");
			langAttribute.Should().Be("lt-LT", "The lang attribute is not equal to lt-LT.");
			Logger.Information("Verified lang attribute matches expected value.");
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
