using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace WebUIAutomation.NUnit_Tests.Report;

public class ReportManager
{
	private static readonly string _reportPath;
	private static readonly ExtentSparkReporter _reporter;
	private static readonly ExtentReports _extent;
	private static ExtentTest _test;

	static ReportManager()
	{
		var baseDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
		_reportPath = Path.Combine(baseDirectory, "Report", "Results", "TestResults.html");

		Directory.CreateDirectory(Path.GetDirectoryName(_reportPath)!);

		_reporter = new ExtentSparkReporter(_reportPath);
		_reporter.Config.ReportName = "Test Execution Report";
		_reporter.Config.DocumentTitle = "Automation Test Results";
		_extent = new ExtentReports();
		_extent.AttachReporter(_reporter);

		_extent.AddSystemInfo("Operating System", Environment.OSVersion.ToString());
		_extent.AddSystemInfo(".NET Version", Environment.Version.ToString());
		_extent.AddSystemInfo("Machine Name", Environment.MachineName);
	}

	public static ExtentReports GetReporter() => _extent;

	public static ExtentTest CreateTest(string testName)
	{
		_test = _extent.CreateTest(testName);
		return _test;
	}

	public static string GetReportPath() => _reportPath;

	public static void Flush()
	{
		_extent.Flush();
	}
}


