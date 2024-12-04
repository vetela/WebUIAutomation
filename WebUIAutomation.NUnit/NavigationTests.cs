using OpenQA.Selenium;
using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Pages;

namespace WebUIAutomation.NUnit;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public class NavigationTests
{
	private HomePage _homePage;

	[SetUp]
	public void TestSetup()
	{
		_homePage = new HomePage();
	}

	[TearDown]
	public void TestTeardown()
	{
		WebDriverSingleton.Instance.QuitDriver();
	}

	[Test]
	[Category("Navigation")]
	public void VerifyAboutEHUPageLoadsCorrectly()
	{
		_homePage.NavigateTo();
		var aboutPage = _homePage.GoToAboutPage();

		Assert.Multiple(() =>
		{
			Assert.That(WebDriverSingleton.Instance.Driver.Url, Is.EqualTo(Constants.AboutPageUrl));
			Assert.That(aboutPage.GetTitle(), Is.EqualTo(Constants.About));
		});

		Assert.That(aboutPage.GetHeaderText(), Is.EqualTo(Constants.About));
	}

	[Test]
	[Category("Search")]
	[TestCase("research")]
	[TestCase("admissions")]
	public void VerifySearchFunctionality(string searchTerm)
	{
		_homePage.NavigateTo();
		var searchQuery = new SearchQuery.Builder().WithTerm(searchTerm).Build();
		var searchResultsPage = _homePage.Search(searchQuery.Term);

		Assert.That(WebDriverSingleton.Instance.Driver.Url, Does.Contain($"/?s={searchQuery.Term.Replace(" ", "+")}"));
		Assert.That(searchResultsPage.ContainsSearchTerm(searchTerm), Is.True, "Search results do not contain expected search term.");
	}

	[Test]
	[Category("Language")]
	public void VerifyLanguageChangeToLithuanian()
	{
		_homePage.NavigateTo();
		_homePage.SwitchToLithuanianLanguage();

		Assert.That(WebDriverSingleton.Instance.Driver.Url, Is.EqualTo(Constants.LithuanianBaseUrl), "The URL does not indicate the language has switched to Lithuanian.");

		var htmlTag = WebDriverSingleton.Instance.Driver.FindElement(By.TagName("html"));
		string langAttribute = htmlTag.GetAttribute("lang");

		Assert.That(langAttribute, Is.EqualTo("lt-LT"), "The lang attribute is not equal to lt-LT.");
	}
}