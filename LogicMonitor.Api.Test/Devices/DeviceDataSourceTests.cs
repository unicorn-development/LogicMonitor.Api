namespace LogicMonitor.Api.Test.Devices;

public class DeviceDataSourceTests : TestWithOutput
{
	public DeviceDataSourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllDeviceDataSourcesAsync()
	{
		var _ = await LogicMonitorClient.GetAllDeviceDataSourcesAsync(WindowsDeviceId, null, CancellationToken.None).ConfigureAwait(false);
	}

	[Fact]
	public async Task DeviceDataSourceTests2()
	{
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU").ConfigureAwait(false);

		// Get all windows devices in the datacenter
		var devices = await LogicMonitorClient.GetDevicesByDeviceGroupFullPathAsync(DeviceGroupFullPath, true).ConfigureAwait(false);
		devices.Should().NotBeNullOrEmpty();
		// We have devices

		// Find datacenter devices with WinCPU datasource
		var deviceDataSources = new List<DeviceDataSource>();
		foreach (var device in devices)
		{
			var deviceDataSourceByDeviceIdAndDataSourceId = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id).ConfigureAwait(false);
			if (deviceDataSourceByDeviceIdAndDataSourceId is null)
			{
				continue;
			}

			deviceDataSources.Add(deviceDataSourceByDeviceIdAndDataSourceId);
		}

		deviceDataSources.Should().NotBeNullOrEmpty();
	}
}
