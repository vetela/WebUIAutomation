using FluentAssertions;
using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Pages;
using static WebUIAutomation.Shared.Logging;

namespace WebUIAutomation.xUnit;

public class SearchFunctionalityTests : IClassFixture<WebDriverFixture>
{
	private readonly HomePage _homePage;

	public SearchFunctionalityTests(WebDriverFixture fixture)
	{
		_homePage = new HomePage();
	}

	[Theory]
	[InlineData("research")]
	[InlineData("admissions")]
	[Trait("Category", "Search")]
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
}
