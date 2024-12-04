using WebUIAutomation.Shared;

namespace WebUIAutomation.xUnit;

public class WebDriverFixture : IDisposable
{
	public WebDriverFixture()
	{
		WebDriverSingleton.Instance.Driver.Navigate().GoToUrl(Constants.BaseUrl);
	}

	public void Dispose()
	{
		WebDriverSingleton.Instance.QuitDriver();
	}
}
