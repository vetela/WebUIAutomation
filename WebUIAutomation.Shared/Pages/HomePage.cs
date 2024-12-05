using OpenQA.Selenium;

namespace WebUIAutomation.Shared.Pages;

public class HomePage : BasePage
{
	private readonly By _aboutLink = By.LinkText("About");
	private readonly By _searchButton = By.ClassName("header-search");
	private readonly By _searchBar = By.ClassName("form-control");
	private readonly By _languageSwitcher = By.CssSelector(".language-switcher");
	private readonly By _lithuanianOption = By.LinkText("LT");

	public void NavigateTo() => Driver.Navigate().GoToUrl("https://en.ehu.lt/");

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
