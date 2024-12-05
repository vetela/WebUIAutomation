using OpenQA.Selenium;

namespace WebUIAutomation.Shared.Pages;

public class SearchResultsPage : BasePage
{
	public bool ContainsSearchTerm(string searchTerm)
	{
		var results = Driver.FindElements(By.ClassName("content"));
		return results.Any(result => result.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
	}
}
