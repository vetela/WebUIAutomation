using OpenQA.Selenium;
using WebUIAutomation.Shared.Pages;
using WebUIAutomation.Shared;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace WebUIAutomation.NUnit_Tests;

[Binding]
public class UserNavigationSteps
{
	private readonly HomePage _homePage;
	private AboutPage _aboutPage;
	private SearchResultsPage _searchResultsPage;

	public UserNavigationSteps()
	{
		_homePage = new HomePage();
	}

	[Given(@"the user is on the Home Page")]
	public void GivenTheUserIsOnTheHomePage()
	{
		_homePage.NavigateTo();
	}

	[When(@"they navigate to the About Page")]
	public void WhenTheyNavigateToTheAboutPage()
	{
		_aboutPage = _homePage.GoToAboutPage();
	}

	[Then(@"the URL should be ""(.*)""")]
	public void ThenTheURLShouldBe(string expectedUrl)
	{
		WebDriverSingleton.Instance.Driver.Url.Should().Be(expectedUrl);
	}

	[Then(@"the page title should be ""(.*)""")]
	public void ThenThePageTitleShouldBe(string expectedTitle)
	{
		_aboutPage.GetTitle().Should().Be(expectedTitle);
	}

	[Then(@"the header text should be ""(.*)""")]
	public void ThenTheHeaderTextShouldBe(string expectedHeader)
	{
		_aboutPage.GetHeaderText().Should().Be(expectedHeader);
	}

	[When(@"they search for ""(.*)""")]
	public void WhenTheySearchFor(string searchTerm)
	{
		var searchQuery = new SearchQuery.Builder().WithTerm(searchTerm).Build();
		_searchResultsPage = _homePage.Search(searchQuery.Term);
	}

	[Then(@"the URL should contain ""(.*)""")]
	public void ThenTheURLShouldContain(string searchTerm)
	{
		WebDriverSingleton.Instance.Driver.Url.Should().Contain($"{searchTerm.Replace(" ", "+")}");
	}

	[Then(@"the search results should include ""(.*)""")]
	public void ThenTheSearchResultsShouldInclude(string searchTerm)
	{
		_searchResultsPage.ContainsSearchTerm(searchTerm).Should().BeTrue("Search results do not contain the expected term.");
	}

	[When(@"they switch the language to Lithuanian")]
	public void WhenTheySwitchTheLanguageToLithuanian()
	{
		_homePage.SwitchToLithuanianLanguage();
	}

	[Then(@"the lang attribute of the page should be ""(.*)""")]
	public void ThenTheLangAttributeOfThePageShouldBe(string expectedLang)
	{
		var htmlTag = WebDriverSingleton.Instance.Driver.FindElement(By.TagName("html"));
		htmlTag.GetAttribute("lang").Should().Be(expectedLang);
	}

	[AfterFeature]
	public static void AfterFeature()
	{
		WebDriverSingleton.Instance?.QuitDriver();
	}
}
