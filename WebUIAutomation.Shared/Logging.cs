using Serilog;
using Serilog.Formatting.Compact;

namespace WebUIAutomation.Shared;

public static class Logging
{
	public static readonly ILogger Logger = new LoggerConfiguration()
		.MinimumLevel.Debug()
		.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
		.WriteTo.File(new CompactJsonFormatter(), "logs/test-log.json", rollingInterval: RollingInterval.Day)
		.Enrich.WithThreadId()
		.CreateLogger();
}
