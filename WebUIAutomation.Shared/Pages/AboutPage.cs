using OpenQA.Selenium;

namespace WebUIAutomation.Shared.Pages;

public class AboutPage : BasePage
{
	public string GetTitle() => Driver.Title;

	public string GetHeaderText()
	{
		var header = Driver.FindElement(By.TagName("h1"));
		return header.Text;
	}
}
