using OpenQA.Selenium;
using WebUIAutomation.Shared.Paths;

namespace WebUIAutomation.xUnit;

public class SearchFunctionalityTests : IClassFixture<WebDriverFixture>
{
	private readonly IWebDriver driver;

	public SearchFunctionalityTests(WebDriverFixture fixture) => this.driver = fixture.driver;

	[Theory]
	[InlineData("research")]
	[InlineData("admissions")]
	[Trait("Category", "Search")]
	public void VerifySearchFunctionality(string searchTerm)
	{
		driver.Navigate().GoToUrl(Constants.BaseUrl);
		var searchButton = driver.FindElement(By.ClassName(Constants.SearchButtonClassName));
		searchButton.Click();
		var searchBar = driver.FindElement(By.ClassName(Constants.SearchBarClassName));
		searchBar.SendKeys(searchTerm);
		searchBar.SendKeys(Keys.Enter);

		Assert.Contains($"/?s={searchTerm.Replace(" ", "+")}", driver.Url);

		var searchResults = driver.FindElements(By.ClassName(Constants.SearchResultsClassName));
		bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
		Assert.True(resultsContainSearchTerm, "Search results do not contain expected search term.");
	}
}
