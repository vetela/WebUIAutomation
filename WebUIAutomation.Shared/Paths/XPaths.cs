namespace WebUIAutomation.Shared.Paths;

public static class XPaths
{
	public const string BaseUrl = "https://en.ehu.lt/";
	public const string LithuanianBaseUrl = "https://lt.ehu.lt/";

	public const string SearchButtonXPath = "//*[@id='masthead']/div[1]/div/div[4]/div";
	public const string SearchBarXPath = "//*[@id='masthead']/div[1]/div/div[4]/div/form/div/input";
	public const string LanguageSwitcherXPath = "//*[@id='masthead']/div[1]/div/div[4]/ul";
	public const string LithuanianOptionXPath = "//*[@id='masthead']/div[1]/div/div[4]/ul/li/ul/li[3]/a";
	public const string SearchResultsXPath = "//*[@id='page']/div[3]";
	public const string AboutLinkXPath = "//*[@id=\"menu-item-16178\"]/a";
}
