﻿namespace LogicMonitor.Api.Test.Logging;

public class PushMetricTests : TestWithOutput
{
	public PushMetricTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task WriteLogAsync_Succeeds()
	{
		var response = await LogicMonitorClient
			.WriteLogAsync(WindowsDeviceId, "Test log message.", default)
			.ConfigureAwait(false);
		response.Should().NotBeNull();
	}

	[Fact]
	public async Task GetLogItems_Succeeds()
	{
		var logItems = await LogicMonitorClient
			.GetLogItemsAsync(default)
			.ConfigureAwait(false);

		logItems.Should().NotBeNull();
	}
}
