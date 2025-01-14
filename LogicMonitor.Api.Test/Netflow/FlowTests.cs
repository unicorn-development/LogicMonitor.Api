using System.Globalization;

namespace LogicMonitor.Api.Test.Netflow;

public class FlowTests : TestWithOutput
{
	public FlowTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
		var endDateTime = DateTime.Today;
		_startDateTimeSeconds = endDateTime.AddDays(-1);
		_endDateTimeSeconds = endDateTime;
	}

	private readonly DateTime _startDateTimeSeconds;
	private readonly DateTime _endDateTimeSeconds;

	[Fact]
	public async Task GetApplications()
	{
		var device = await LogicMonitorClient
			.GetDevicesPageAsync(new Filter<Device>(), default)
			.ConfigureAwait(true);

		List<Device> netflowDeviceList = [];

		foreach (var d in device.Items)
		{
			if (d.EnableNetflow)
			{
				netflowDeviceList.Add(d);
			}
		}

		var flowApplications = await LogicMonitorClient
			.GetFlowApplicationsPageAsync(
				new FlowApplicationsRequest
				{
					DeviceId = netflowDeviceList[0].Id
				},
				default
			)
			.ConfigureAwait(true);

		// Make sure that some are returned
		flowApplications.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task GetApplicationsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync(default)
			.ConfigureAwait(true);

		var flowApplications = await LogicMonitorClient
			.GetDeviceGroupFlowApplicationsPageAsync(
				new DeviceGroupFlowApplicationsRequest
				{
					TimePeriod = TimePeriod.Zoom,
					DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
					SortDirection = SortDirection.Ascending,
					SortFlowField = FlowField.Usage,
					Take = 100,
					Skip = 0,
					FlowDirection = FlowDirection.All,
					QosType = "all",
					StartDateTime = DateTime.UtcNow.AddDays(-2),
					EndDateTime = DateTime.UtcNow.AddDays(-1),
				},
				default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		flowApplications.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetBandwidthsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync(default)
			.ConfigureAwait(true);

		var flowBandwidths = await LogicMonitorClient.GetDeviceGroupFlowBandwidthsPageAsync(new DeviceGroupFlowBandwidthsRequest
		{
			TimePeriod = TimePeriod.Zoom,
			DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
			SortDirection = SortDirection.Ascending,
			SortFlowField = FlowField.Usage,
			Take = 100,
			Skip = 0,
			FlowDirection = FlowDirection.All,
			StartDateTime = DateTime.UtcNow.AddDays(-2),
			EndDateTime = DateTime.UtcNow.AddDays(-1)
		}, default)
		.ConfigureAwait(true);

		// Make sure that some are returned
		flowBandwidths.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetFlowsForDeviceGroup()
	{
		var device = await GetNetflowDeviceAsync(default)
			.ConfigureAwait(true);

		var flows = await LogicMonitorClient.GetDeviceGroupFlowsPageAsync(new DeviceGroupFlowsRequest
		{
			TimePeriod = TimePeriod.Zoom,
			DeviceGroupId = int.Parse(device.DeviceGroupIdsString.Split(",")[0], CultureInfo.InvariantCulture),
			SortDirection = SortDirection.Ascending,
			SortFlowField = FlowField.Usage,
			Take = 100,
			Skip = 0,
			FlowDirection = FlowDirection.All,
			StartDateTime = DateTime.UtcNow.AddDays(-2),
			EndDateTime = DateTime.UtcNow.AddDays(-1)
		}, default)
		.ConfigureAwait(true);

		// Make sure that some are returned
		flows.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetFlowApplications()
	{
		var device = await GetNetflowDeviceAsync(default).ConfigureAwait(true);
		var flowEndpoints = await LogicMonitorClient.GetFlowApplicationsPageAsync(new FlowApplicationsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			DeviceId = device.Id
		}, default
		).ConfigureAwait(true);

		// Make sure that some are returned
		flowEndpoints.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task GetFlows()
	{
		var device = await GetNetflowDeviceAsync(default).ConfigureAwait(true);
		var flows = await LogicMonitorClient.GetFlowsPageAsync(new FlowsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			DeviceId = device.Id
		}, default
		).ConfigureAwait(true);

		// Make sure that some are returned
		flows.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task GetPorts()
	{
		var device = await GetNetflowDeviceAsync(default)
			.ConfigureAwait(true);
		var flowPorts = await LogicMonitorClient.GetFlowPortsPageAsync(new FlowPortsRequest
		{
			TimePeriod = TimePeriod.OneDay,
			DeviceId = device.Id
		}, default
		).ConfigureAwait(true);

		// Make sure that some are returned
		flowPorts.Items.Should().NotBeNullOrEmpty();

		// TODO Make sure that flows are unique in some way
		//((flows.Select(flow => flow.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task GetZoomTimeFlows()
	{
		var device = await GetNetflowDeviceAsync(default)
			.ConfigureAwait(true);
		var flows = await LogicMonitorClient.GetFlowsPageAsync(new FlowsRequest
		{
			DeviceId = NetflowDeviceId,
			TimePeriod = TimePeriod.Zoom,
			StartDateTime = _startDateTimeSeconds,
			EndDateTime = _endDateTimeSeconds
		}, default
		).ConfigureAwait(true);

		// Make sure that some are returned
		flows.Items.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetDeviceFlowInformation()
	{
		var interfaces = await LogicMonitorClient
			.GetDeviceFlowInterfacesPageAsync(NetflowDeviceId, new Filter<FlowInterface>(), default)
			.ConfigureAwait(true);
		interfaces.Items.Should().NotBeNullOrEmpty();
	}
}
