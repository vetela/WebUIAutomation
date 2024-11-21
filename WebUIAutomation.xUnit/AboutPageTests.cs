using OpenQA.Selenium;
using WebUIAutomation.Shared.Paths;

namespace WebUIAutomation.xUnit;

public class AboutPageTests : IClassFixture<WebDriverFixture>
{
	private readonly IWebDriver driver;

	public AboutPageTests(WebDriverFixture fixture) => this.driver = fixture.driver;

	[Fact]
	[Trait("Category", "Navigation")]
	public void VerifyAboutEHUPageLoadsCorrectly()
	{
		driver.Navigate().GoToUrl(XPaths.BaseUrl);
		var aboutLink = driver.FindElement(By.XPath(XPaths.AboutLinkXPath));
		aboutLink.Click();

		Assert.Equal($"{XPaths.BaseUrl}about/", driver.Url);
		Assert.Equal("About", driver.Title);
	}
}
