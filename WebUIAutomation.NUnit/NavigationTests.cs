using OpenQA.Selenium;
using FluentAssertions;
using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Pages;
using static WebUIAutomation.Shared.Logging;

namespace WebUIAutomation.NUnit;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public class NavigationTests
{
	private HomePage _homePage;

	[SetUp]
	public void TestSetup()
	{
		Logger.Information("Test setup initiated.");
		_homePage = new HomePage();
	}

	[TearDown]
	public void TestTeardown()
	{
		Logger.Information("Test teardown initiated.");
		WebDriverSingleton.Instance.QuitDriver();
	}

	[Test]
	[Category("Navigation")]
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

	[Test]
	[Category("Search")]
	[TestCase("research")]
	[TestCase("admissions")]
	public void VerifySearchFunctionality(string searchTerm)
	{
		var testName = nameof(VerifySearchFunctionality);
		Logger.Information("Starting test: {TestName} with search term: {SearchTerm}", testName, searchTerm);

		try
		{
			_homePage.NavigateTo();
			Logger.Debug("Navigated to Home Page.");

			var searchQuery = new SearchQuery.Builder().WithTerm(searchTerm).Build();
			var searchResultsPage = _homePage.Search(searchQuery.Term);
			Logger.Debug("Performed search with term: {SearchTerm}", searchQuery.Term);

			WebDriverSingleton.Instance.Driver.Url.Should().Contain($"/?s={searchQuery.Term.Replace(" ", "+")}");
			Logger.Information("Verified URL contains search term.");

			searchResultsPage.ContainsSearchTerm(searchTerm).Should().BeTrue("Search results do not contain the expected term.");
			Logger.Information("Verified search results contain the expected term.");
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

	[Test]
	[Category("Language")]
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

			var currentUrl = WebDriverSingleton.Instance.Driver.Url;
			currentUrl.Should().Be("https://lt.ehu.lt/", "The URL does not indicate the language has switched to Lithuanian.");
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