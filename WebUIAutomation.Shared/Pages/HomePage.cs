using OpenQA.Selenium;

namespace WebUIAutomation.Shared.Pages;

public class HomePage : BasePage
{
	private readonly By _aboutLink = By.LinkText(Constants.About);
	private readonly By _searchButton = By.ClassName(Constants.SearchButtonClassName);
	private readonly By _searchBar = By.ClassName(Constants.SearchBarClassName);
	private readonly By _languageSwitcher = By.CssSelector(Constants.LanguageSwitcherCss);
	private readonly By _lithuanianOption = By.LinkText(Constants.LithuanianLanguage);

	public void NavigateTo() => Driver.Navigate().GoToUrl(Constants.BaseUrl);

	public AboutPage GoToAboutPage()
	{
		Driver.FindElement(_aboutLink).Click();
		return new AboutPage();
	}

	public SearchResultsPage Search(string searchTerm)
	{
		Driver.FindElement(_searchButton).Click();
		Driver.FindElement(_searchBar).SendKeys(searchTerm + Keys.Enter);
		return new SearchResultsPage();
	}

	public void SwitchToLithuanianLanguage()
	{
		Driver.FindElement(_languageSwitcher).Click();
		Driver.FindElement(_lithuanianOption).Click();
	}
}
