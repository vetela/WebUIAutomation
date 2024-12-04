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
		driver.Navigate().GoToUrl(Constants.BaseUrl);
		var aboutLink = driver.FindElement(By.LinkText(Constants.AboutLink));
		aboutLink.Click();

		Assert.Equal($"{Constants.BaseUrl}about/", driver.Url);
		Assert.Equal("About", driver.Title);
	}
}
