using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebUIAutomation.Shared;

public class BaseTest
{
	protected ThreadLocal<IWebDriver> _driver = new();
	protected IWebDriver driver => _driver.Value;

	public void Setup()
	{
		_driver.Value = new ChromeDriver();
		_driver.Value.Manage().Window.Maximize();
	}

	public void Teardown()
	{
		_driver.Value?.Quit();
		_driver.Value?.Dispose();
	}
}
