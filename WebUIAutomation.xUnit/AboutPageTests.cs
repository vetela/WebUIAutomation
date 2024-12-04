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

		Assert.Equal(Constants.AboutPageUrl, WebDriverSingleton.Instance.Driver.Url);
		Assert.Equal(Constants.About, aboutPage.GetTitle());
		Assert.Equal(Constants.About, aboutPage.GetHeaderText());
	}
}
