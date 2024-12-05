using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Pages;

namespace WebUIAutomation.xUnit;

public class AboutPageTests : IClassFixture<WebDriverFixture>
{
	private readonly HomePage _homePage;
	public AboutPageTests(WebDriverFixture fixture) => _homePage = new HomePage();

	[Fact]
	[Trait("Category", "Navigation")]
	public void VerifyAboutEHUPageLoadsCorrectly()
	{
		_homePage.NavigateTo();
		var aboutPage = _homePage.GoToAboutPage();

		Assert.Equal("https://en.ehu.lt/about/", WebDriverSingleton.Instance.Driver.Url);
		Assert.Equal("About", aboutPage.GetTitle());
		Assert.Equal("About", aboutPage.GetHeaderText());
	}
}
