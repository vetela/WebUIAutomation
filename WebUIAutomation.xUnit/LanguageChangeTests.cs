using OpenQA.Selenium;
using WebUIAutomation.Shared.Paths;

namespace WebUIAutomation.xUnit;


public class LanguageChangeTests : IClassFixture<WebDriverFixture>
{
	private readonly IWebDriver driver;

	public LanguageChangeTests(WebDriverFixture fixture) => this.driver = fixture.driver;

	[Fact]
	[Trait("Category", "Language")]
	public void VerifyLanguageChangeToLithuanian()
	{
		driver.Navigate().GoToUrl(XPaths.BaseUrl);

		var languageSwitcher = driver.FindElement(By.XPath(XPaths.LanguageSwitcherXPath));
		languageSwitcher.Click();

		var lithuanianOption = driver.FindElement(By.XPath(XPaths.LithuanianOptionXPath));
		lithuanianOption.Click();

		Assert.Equal(XPaths.LithuanianBaseUrl, driver.Url);

		var htmlTag = driver.FindElement(By.TagName("html"));
		string langAttribute = htmlTag.GetAttribute("lang");

		Assert.Equal("lt-LT", langAttribute);
	}
}
