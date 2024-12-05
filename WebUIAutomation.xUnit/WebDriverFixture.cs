using WebUIAutomation.Shared;

namespace WebUIAutomation.xUnit;

public class WebDriverFixture : IDisposable
{
	public WebDriverFixture()
	{
		WebDriverSingleton.Instance.Driver.Navigate().GoToUrl("https://en.ehu.lt/");
	}

	public void Dispose()
	{
		WebDriverSingleton.Instance.QuitDriver();
	}
}
