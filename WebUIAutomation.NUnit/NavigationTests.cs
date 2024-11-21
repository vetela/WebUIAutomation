using OpenQA.Selenium;
using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Paths;

namespace WebUIAutomation.NUnit;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public class NavigationTests : BaseTest
{
	[SetUp]
	public void TestSetup() => Setup();

	[TearDown]
	public void TestTeardown() => Teardown();

	[Test]
	[Category("Navigation")]
	public void VerifyAboutEHUPageLoadsCorrectly()
	{
		driver.Navigate().GoToUrl(XPaths.BaseUrl);
		IWebElement aboutLink = driver.FindElement(By.XPath(XPaths.AboutLinkXPath));
		aboutLink.Click();

		Assert.Multiple(() =>
		{
			Assert.That(driver.Url, Is.EqualTo($"{XPaths.BaseUrl}about/"));
			Assert.That(driver.Title, Is.EqualTo("About"));
		});

		IWebElement header = driver.FindElement(By.TagName("h1"));
		Assert.That(header.Text, Is.EqualTo("About"));
	}

	[Test]
	[Category("Search")]
	[TestCase("research")]
	[TestCase("admissions")]
	public void VerifySearchFunctionality(string searchTerm)
	{
		driver.Navigate().GoToUrl(XPaths.BaseUrl);

		var searchButton = driver.FindElement(By.XPath(XPaths.SearchButtonXPath));
		searchButton.Click();

		var searchBar = driver.FindElement(By.XPath(XPaths.SearchBarXPath));
		searchBar.SendKeys(searchTerm);
		searchBar.SendKeys(Keys.Enter);

		Assert.That(driver.Url, Does.Contain($"/?s={searchTerm.Replace(" ", "+")}"), "Search query mismatch");

		var searchResults = driver.FindElements(By.XPath(XPaths.SearchResultsXPath));
		bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
		Assert.That(resultsContainSearchTerm, Is.True, "Search results do not contain expected search term.");
	}

	[Test]
	[Category("Language")]
	public void VerifyLanguageChangeToLithuanian()
	{
		driver.Navigate().GoToUrl(XPaths.BaseUrl);

		var languageSwitcher = driver.FindElement(By.XPath(XPaths.LanguageSwitcherXPath));
		languageSwitcher.Click();

		var lithuanianOption = driver.FindElement(By.XPath(XPaths.LithuanianOptionXPath));
		lithuanianOption.Click();

		Assert.That(driver.Url, Is.EqualTo(XPaths.LithuanianBaseUrl), "The URL does not indicate the language has switched to Lithuanian.");

		var htmlTag = driver.FindElement(By.TagName("html"));
		string langAttribute = htmlTag.GetAttribute("lang");

		Assert.That(langAttribute, Is.EqualTo("lt-LT"), "The lang attribute is not equal to lt-LT.");
	}

	//[Test]
	//public void VerifyContactFormSubmission()
	//{
	//	// Impossible to implement task due to incorrect task description
	//	Assert.That(true);
	//}
}