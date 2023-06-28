namespace LogicMonitor.Api.Test.Devices;

public class UnmonitoredDeviceTests : TestWithOutput
{
	public UnmonitoredDeviceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetUnmonitoredDevices()
	{
		var unmonitoredDevices = await LogicMonitorClient
			.GetUnmonitoredDeviceAsync(new Filter<UnmonitoredDevice>(), default)
			.ConfigureAwait(false);
		unmonitoredDevices.Should().NotBeNull();
	}
}
