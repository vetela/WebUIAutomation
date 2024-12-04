using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WebUIAutomation.Shared;

public class WebDriverSingleton
{
	private static readonly Lazy<WebDriverSingleton> _instance = new(() => new WebDriverSingleton());
	private ThreadLocal<IWebDriver> _driver = new();

	public static WebDriverSingleton Instance => _instance.Value;

	public IWebDriver Driver
	{
		get
		{
			if (_driver.Value == null)
			{
				_driver.Value = new ChromeDriver();
				_driver.Value.Manage().Window.Maximize();
			}
			return _driver.Value;
		}
	}

	private WebDriverSingleton() { }

	public void QuitDriver()
	{
		_driver.Value?.Quit();
		_driver.Value?.Dispose();
		_driver.Value = null;
	}
}
