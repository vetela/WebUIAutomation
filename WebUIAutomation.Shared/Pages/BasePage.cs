using OpenQA.Selenium;

namespace WebUIAutomation.Shared.Pages;

public abstract class BasePage
{
	protected IWebDriver Driver => WebDriverSingleton.Instance.Driver;
}
