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
		driver.Navigate().GoToUrl(Constants.BaseUrl);

		var languageSwitcher = driver.FindElement(By.CssSelector(Constants.LanguageSwitcherCss));
		languageSwitcher.Click();

		var lithuanianOption = driver.FindElement(By.LinkText(Constants.LithuanianLanguage));
		lithuanianOption.Click();

		Assert.Equal(Constants.LithuanianBaseUrl, driver.Url);

		var htmlTag = driver.FindElement(By.TagName("html"));
		string langAttribute = htmlTag.GetAttribute("lang");

		Assert.Equal("lt-LT", langAttribute);
	}
}
