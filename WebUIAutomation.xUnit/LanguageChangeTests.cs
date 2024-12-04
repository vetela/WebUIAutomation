using OpenQA.Selenium;
using WebUIAutomation.Shared;
using WebUIAutomation.Shared.Pages;

namespace WebUIAutomation.xUnit;


public class LanguageChangeTests : IClassFixture<WebDriverFixture>
{
	private readonly HomePage _homePage;

	public LanguageChangeTests(WebDriverFixture fixture)
	{
		_homePage = new HomePage();
	}

	[Fact]
	[Trait("Category", "Language")]
	public void VerifyLanguageChangeToLithuanian()
	{
		_homePage.NavigateTo();
		_homePage.SwitchToLithuanianLanguage();

		Assert.Equal(Constants.LithuanianBaseUrl, WebDriverSingleton.Instance.Driver.Url);

		var htmlTag = WebDriverSingleton.Instance.Driver.FindElement(By.TagName("html"));
		string langAttribute = htmlTag.GetAttribute("lang");

		Assert.Equal("lt-LT", langAttribute);
	}
}
