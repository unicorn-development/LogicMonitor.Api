using System.Globalization;

namespace LogicMonitor.Api.Test.Data;

public class DataTests : TestWithOutput
{
	public DataTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetForecastGraphData()
	{
		var device = await GetWindowsDeviceAsync(CancellationToken.None)
			.ConfigureAwait(false);
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken.None)
			.ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		var dataSourceGraphs = await LogicMonitorClient
			.GetDataSourceGraphsAsync(dataSource!.Id, CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(
				device.Id,
				deviceDataSource.Id,
				cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
		var deviceDataSourceInstance = deviceDataSourceInstances[0];
		var dataSourceGraph = dataSourceGraphs[0];
		var virtualDataPoint = dataSourceGraph.DataPoints[0];
		var forecastGraphData = await LogicMonitorClient
			.GetForecastGraphDataAsync(
				new ForecastDataRequest
				{
					TrainingTimePeriod = TrainingTimePeriod.SixMonths,
					ForecastTimePeriod = ForecastTimePeriod.OneMonth,
					DataSourceInstanceId = deviceDataSourceInstance.Id,
					GraphId = dataSourceGraph.Id,
					DataPointLabel = virtualDataPoint.Name
				},
				CancellationToken.None)
			.ConfigureAwait(false);

		forecastGraphData.TrainingGraphData.Lines.Should().HaveCount(1);
		forecastGraphData.ForecastedGraphData.Lines.Count.Should().Be(3);
	}

	[Fact]
	public async Task GetOverviewGraphData()
	{
		var device = await GetSnmpDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		device.Should().NotBeNull();
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("snmp64_If-", CancellationToken.None).ConfigureAwait(false);
		dataSource.Should().NotBeNull();
		var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken.None).ConfigureAwait(false);
		deviceDataSource.Should().NotBeNull();
		var deviceDataSourceInstanceGroups = await LogicMonitorClient.GetDeviceDataSourceInstanceGroupsAsync(device.Id, deviceDataSource.Id, CancellationToken.None).ConfigureAwait(false);
		deviceDataSourceInstanceGroups.Should().NotBeNull();
		deviceDataSourceInstanceGroups.Should().NotBeNullOrEmpty();
		var deviceDataSourceInstanceGroup = deviceDataSourceInstanceGroups.Skip(1).First();
		var deviceDataSourceInstanceGroupRefetch = await LogicMonitorClient.GetDeviceDataSourceInstanceGroupByNameAsync(device.Id, deviceDataSource.Id, deviceDataSourceInstanceGroup.Name, CancellationToken.None).ConfigureAwait(false);
		deviceDataSourceInstanceGroupRefetch.Should().NotBeNull();
		deviceDataSourceInstanceGroupRefetch.Name.Should().Be(deviceDataSourceInstanceGroup.Name);

		var overviewGraph = await LogicMonitorClient.GetDeviceOverviewGraphByNameAsync(device.Id, deviceDataSource.Id, "Top 10 Interfaces by Total Packets", CancellationToken.None).ConfigureAwait(false);
		overviewGraph.Should().NotBeNull();
		var graphDataRequest = new DeviceDataSourceGraphDataRequest
		{
			DataSourceInstanceGroupId = deviceDataSourceInstanceGroup.Id,
			OverviewGraphId = overviewGraph.Id,
			StartDateTime = DateTime.UtcNow.FirstDayOfLastMonth(),
			EndDateTime = DateTime.UtcNow.LastDayOfLastMonth(),
			TimePeriod = TimePeriod.Zoom,
			Width = 500
		};
		graphDataRequest.Validate();
		var graphData = await LogicMonitorClient.GetGraphDataAsync(graphDataRequest, CancellationToken.None).ConfigureAwait(false);
		graphData.Should().NotBeNull();
	}

	/// <summary>
	/// Netflow Data
	/// </summary>
	[Fact]
	public async Task GetNetflowGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var netflowDevice = await GetNetflowDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		var _ = await LogicMonitorClient.GetGraphDataAsync(new NetflowGraphDataRequest
		{
			DeviceId = netflowDevice.Id,
			StartDateTime = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(-1),
			EndDateTime = new DateTime(utcNow.Year, utcNow.Month, 1),
			NetflowFilter = new NetflowFilter(),
			TimePeriod = TimePeriod.Zoom
		}, CancellationToken.None).ConfigureAwait(false);
	}

	/// <summary>
	/// Netflow Data
	/// </summary>
	[Fact]
	public async Task GetNetflowGraphDataForDeviceGroup()
	{
		var utcNow = DateTime.UtcNow;

		// Get the configured Netflow Device
		var netflowDevice = await GetNetflowDeviceAsync(CancellationToken.None).ConfigureAwait(false);

		// Create the request
		var request = new NetflowDeviceGroupGraphDataRequest
		{
			DeviceGroupId = int.Parse(netflowDevice.DeviceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
			StartDateTime = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(-1),
			EndDateTime = new DateTime(utcNow.Year, utcNow.Month, 1),
			TimePeriod = TimePeriod.Zoom
		};

		// Send the request
		var data = await LogicMonitorClient.GetGraphDataAsync(request, CancellationToken.None).ConfigureAwait(false);

		// Check there is at least one line of data
		data.Lines.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetGraphData_X250()
	{
		LogicMonitorClient.UseCache = true;
		var utcNow = DateTime.UtcNow;
		var startDateTime = utcNow.FirstDayOfLastMonth();
		var device = await GetWindowsDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken.None).ConfigureAwait(false);
		dataSource.Should().NotBeNull();

		var dataSourceGraph = await LogicMonitorClient.GetDataSourceGraphByNameAsync(dataSource!.Id, "CPU Usage", CancellationToken.None).ConfigureAwait(false);
		dataSourceGraph.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken.None).ConfigureAwait(false);
		var deviceDataSourceInstances = await LogicMonitorClient.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>(), CancellationToken.None).ConfigureAwait(false);
		var deviceGraphDataRequest = new DeviceDataSourceInstanceGraphDataRequest
		{
			DeviceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = utcNow.LastDayOfLastMonth()
		};
		var stopwatch = Stopwatch.StartNew();
		for (var n = 0; n < 250; n++)
		{
			Logger.LogInformation("{N:000}: {ElapsedMS:00000}ms", n, stopwatch.ElapsedMilliseconds);
			await LogicMonitorClient.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken.None).ConfigureAwait(false);
		}
	}

	[Fact]
	public async Task GetGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var startDateTime = utcNow.FirstDayOfLastMonth();
		var device = await GetWindowsDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		device.Should().NotBeNull();

		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken.None)
			.ConfigureAwait(false);
		dataSource.Should().NotBeNull();

		var dataSourceGraph = await LogicMonitorClient
			.GetDataSourceGraphByNameAsync(dataSource.Id, "CPU Usage", CancellationToken.None)
			.ConfigureAwait(false);
		dataSourceGraph.Should().NotBeNull();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken.None)
			.ConfigureAwait(false);
		deviceDataSource.Should().NotBeNull();

		var deviceDataSourceInstances = await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(device.Id, deviceDataSource.Id, new Filter<DeviceDataSourceInstance>(), CancellationToken.None)
			.ConfigureAwait(false);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();

		var deviceGraphDataRequest = new DeviceDataSourceInstanceGraphDataRequest
		{
			DeviceDataSourceInstanceId = deviceDataSourceInstances.Single().Id,
			DataSourceGraphId = dataSourceGraph.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = utcNow.LastDayOfLastMonth()
		};

		//  Ensure Caching is enabled
		LogicMonitorClient.UseCache = true;

		var graphData = await LogicMonitorClient.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken.None).ConfigureAwait(false);
		graphData.Lines.Should().NotBeNullOrEmpty();
		graphData.StartTimeUtc.Should().Be(startDateTime);
		graphData.Lines[0].ColorString.Should().NotBeNull();

		// Ensure that subsequent fetches are fast
		var stopwatch = Stopwatch.StartNew();
		graphData = await LogicMonitorClient.GetGraphDataAsync(deviceGraphDataRequest, CancellationToken.None).ConfigureAwait(false);
		graphData.Should().NotBeNull();
		stopwatch.Stop();
		stopwatch.ElapsedMilliseconds.Should().BeLessThan(50);
	}

	[Fact]
	public async Task GetWidgetGraphData()
	{
		var utcNow = DateTime.UtcNow;
		var startDateTime = utcNow.FirstDayOfLastMonth();
		var dashboard = await GetAllWidgetsDashboardAsync(CancellationToken.None).ConfigureAwait(false);
		dashboard.Should().NotBeNull();

		var widgets = await LogicMonitorClient.GetWidgetsByDashboardIdAsync(dashboard.Id, CancellationToken.None).ConfigureAwait(false);
		widgets.Should().NotBeNull();
		widgets.Should().NotBeNullOrEmpty();

		var firstCustomGraphWidget = widgets.Find(w => w.Type == "cgraph");
		firstCustomGraphWidget.Should().NotBeNull();

		var widgetGraphDataRequest = new WidgetGraphDataRequest
		{
			WidgetId = firstCustomGraphWidget.Id,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = startDateTime,
			EndDateTime = utcNow.LastDayOfLastMonth()
		};
		var graphData = await LogicMonitorClient.GetGraphDataAsync(widgetGraphDataRequest, CancellationToken.None).ConfigureAwait(false);
		graphData.Lines.Should().NotBeNullOrEmpty();
		graphData.StartTimeUtc.Should().Be(startDateTime);
		graphData.Lines[0].ColorString.Should().NotBeNull();
	}

	[Fact]
	public async Task GetWinCpuDeviceDataSourceInstancesFromDev()
	{
		var device = await GetWindowsDeviceAsync(CancellationToken.None).ConfigureAwait(false);
		var dataSource = await LogicMonitorClient.GetDataSourceByUniqueNameAsync("WinCPU", CancellationToken.None).ConfigureAwait(false);
		var deviceDataSource = await LogicMonitorClient.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(device.Id, dataSource.Id, CancellationToken.None).ConfigureAwait(false);
		var deviceDataSourceInstances = await LogicMonitorClient
				.GetAllDeviceDataSourceInstancesAsync(
					device.Id,
					deviceDataSource.Id,
					new Filter<DeviceDataSourceInstance>
					{
						Take = 300,
						ExtraFilters = new List<FilterItem<DeviceDataSourceInstance>>
						{
								new Eq<DeviceDataSourceInstance>(nameof(DeviceDataSourceInstance.StopMonitoring), false)
						},
						Order = new Order<DeviceDataSourceInstance> { Property = nameof(DeviceDataSourceInstance.Name), Direction = OrderDirection.Asc }
					}, CancellationToken.None).ConfigureAwait(false);
		deviceDataSourceInstances.Should().NotBeNull();
		deviceDataSourceInstances.Should().NotBeNullOrEmpty();
	}
}
