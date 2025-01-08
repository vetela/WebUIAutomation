using WebUIAutomation.Shared.Pages;
using WebUIAutomation.Shared;
using FluentAssertions;
using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using System.Runtime.InteropServices;
using WebUIAutomation.NUnit_Tests.Report;
using OpenQA.Selenium;

namespace WebUIAutomation.NUnit_Tests;


[TestFixture]
public class NavigationTestsForReport
{
	private HomePage _homePage;
	private ExtentTest _test;

	[OneTimeSetUp]
	public void SuiteSetup()
	{
		var reporter = ReportManager.GetReporter();
		reporter.AddSystemInfo("Operating System", RuntimeInformation.OSDescription);
		reporter.AddSystemInfo(".NET Version", RuntimeInformation.FrameworkDescription);
	}

	[SetUp]
	public void TestSetup()
	{
		_homePage = new HomePage();
		var testName = TestContext.CurrentContext.Test.Name;
		_test = ReportManager.CreateTest(testName);
	}

	[TearDown]
	public void TestTeardown()
	{
		var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
		var testError = TestContext.CurrentContext.Result.Message;

		switch (testStatus)
		{
			case TestStatus.Passed:
				_test.Pass("Test passed successfully.");
				break;
			case TestStatus.Failed:
				_test.Fail($"Test failed. Error: {testError}");
				CaptureScreenshot(_test);
				break;
			case TestStatus.Skipped:
				_test.Skip("Test was skipped.");
				break;
		}

		ReportManager.Flush();
		WebDriverSingleton.Instance.QuitDriver();
	}

	[Test]
	[Category("Navigation")]
	public void VerifyAboutEHUPageLoadsCorrectly() // Pass
	{
		_test.Info("Navigating to Home Page.");
		_homePage.NavigateTo();
		_test.Info("Navigating to About Page.");
		var aboutPage = _homePage.GoToAboutPage();

		_test.Info("Verifying page title.");
		aboutPage.GetTitle().Should().Be("About");
	}

	[Test]
	[Category("Search")]
	public void VerifySearchFunctionalityFails() // Fail
	{
		_test.Info("Navigating to Home Page.");
		_homePage.NavigateTo();

		_test.Info("Performing search with a nonexistent term.");
		var searchQuery = new SearchQuery.Builder().WithTerm("nonexistent").Build();
		var searchResultsPage = _homePage.Search(searchQuery.Term);

		_test.Info("Verifying search results contain a different term (expected to fail).");
		searchResultsPage.ContainsSearchTerm("differentTerm").Should().BeTrue();
	}

	[Test]
	[Category("Language")]
	[Ignore("Skipping this test for demonstration purposes.")]
	public void VerifyLanguageChangeToLithuanian() // Skip
	{
		_test.Skip("This test is marked as inconclusive.");
		Assert.Inconclusive("This test is marked as inconclusive.");
	}

	private void CaptureScreenshot(ExtentTest test)
	{
		try
		{
			var reportDirectory = Path.GetDirectoryName(ReportManager.GetReportPath());
			var screenshotsDirectory = Path.Combine(reportDirectory!, "Screenshots");
			Directory.CreateDirectory(screenshotsDirectory);

			var screenshotFilePath = Path.Combine(screenshotsDirectory, $"{TestContext.CurrentContext.Test.Name}.png");

			if (WebDriverSingleton.Instance.Driver is ITakesScreenshot takesScreenshot)
			{
				var screenshot = takesScreenshot.GetScreenshot();
				screenshot.SaveAsFile(screenshotFilePath);

				test.AddScreenCaptureFromPath(screenshotFilePath);
				test.Info($"Screenshot captured for failure: {screenshotFilePath}");
			}
			else
			{
				test.Warning("Current WebDriver does not support screenshots.");
			}
		}
		catch (Exception ex)
		{
			test.Warning($"Unable to capture screenshot. Error: {ex.Message}");
		}
	}
}

