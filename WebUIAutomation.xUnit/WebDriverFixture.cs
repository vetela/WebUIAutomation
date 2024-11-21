using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WebUIAutomation.xUnit;

public class WebDriverFixture : IDisposable
{
	protected ThreadLocal<IWebDriver> _driver = new();
	public IWebDriver driver => _driver.Value!;

	public WebDriverFixture()
	{
		_driver.Value = new ChromeDriver();
		_driver.Value.Manage().Window.Maximize();
	}

	public void Dispose()
	{
		_driver.Value?.Quit();
		_driver.Value?.Dispose();
	}
}
