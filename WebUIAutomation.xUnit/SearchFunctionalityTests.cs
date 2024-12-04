using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Pages;

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
		_homePage.NavigateTo();
		var searchQuery = new SearchQuery.Builder().WithTerm(searchTerm).Build();
		var searchResultsPage = _homePage.Search(searchQuery.Term);

		Assert.Contains($"/?s={searchQuery.Term.Replace(" ", "+")}", WebDriverSingleton.Instance.Driver.Url);
		Assert.True(searchResultsPage.ContainsSearchTerm(searchTerm), "Search results do not contain expected search term.");
	}
}
