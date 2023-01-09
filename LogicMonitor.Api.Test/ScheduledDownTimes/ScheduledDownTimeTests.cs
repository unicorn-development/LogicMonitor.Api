namespace LogicMonitor.Api.Test.ScheduledDownTimes;

public class ScheduledDownTimeTests : TestWithOutput
{
	public ScheduledDownTimeTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetDeviceScheduledDownTimes()
	{
		var sdts = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceSDT"),
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.IsEffective), false),
				}
		}, CancellationToken.None).ConfigureAwait(false);
		Assert.All(sdts, sdt => Assert.False(sdt.IsEffective));
	}

	[Fact]
	public async Task GetWebsiteGroupScheduledDownTimes_UsingSubUrl_Succeeds()
	{
		// Ensure that a website group SDT exists with an unusual comment
		var commentGuid = Guid.NewGuid();
		var initialComment = $"ABC {commentGuid} DEF";
		var websiteGroupId = 1;
		var sdtCreationDto = new WebsiteGroupScheduledDownTimeCreationDto(1)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient
			.CreateAsync(sdtCreationDto, CancellationToken.None)
			.ConfigureAwait(false);
		createdSdt.Comment.Should().Be(initialComment);
		createdSdt.WebsiteGroupId.Should().Be(websiteGroupId);

		var subUrl = $"sdt/sdts?filter=type:\"WebsiteGroupSDT\",comment~\"{commentGuid}\"";

		var scheduledDownTimes = await LogicMonitorClient
			.GetAllAsync<ScheduledDownTime>(subUrl, CancellationToken.None)
			.ConfigureAwait(false);
		scheduledDownTimes.Should().AllSatisfy(sdt =>
		{
			sdt.Comment.Should().Contain(commentGuid.ToString());
			sdt.Type.Should().Be(ScheduledDownTimeType.WebsiteGroup);
		});

		await LogicMonitorClient
			.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task GetHistoricDeviceScheduledDownTimes()
	{
		// Device
		var testsdts =
			await LogicMonitorClient.GetDeviceHistorySdts(1053, default)
			.ConfigureAwait(false);
		testsdts.Should().NotBeNull();

		// Device Group
		var _deviceGroupHistorySdts =
			await LogicMonitorClient.GetDeviceGroupHistorySdts(1516, default)
			.ConfigureAwait(false);
		_deviceGroupHistorySdts.Should().NotBeNull();

		// Device
		var deviceHistorySdts =
			await LogicMonitorClient.GetDeviceHistorySdts(1765, default)
			.ConfigureAwait(false);
		deviceHistorySdts.Should().NotBeNull();

		// Device Data Source
		var deviceDataSourceHistorySdts =
			await LogicMonitorClient.GetDeviceDataSourceHistorySdts(1765, 98562, default)
			.ConfigureAwait(false);
		deviceDataSourceHistorySdts.Should().NotBeNull();

		// Device Data Source Instance
		var deviceDataSourceInstanceHistorySdts =
			await LogicMonitorClient.GetDeviceDataSourceInstanceHistorySdts(1765, 98562, 244662832, default)
			.ConfigureAwait(false);
		deviceDataSourceInstanceHistorySdts.Should().NotBeNull();

		// Website Group
		var websiteGroupHistorySdts =
			await LogicMonitorClient.GetWebsiteGroupHistorySdts(20, default)
			.ConfigureAwait(false);
		websiteGroupHistorySdts.Should().NotBeNull();

		// Website
		var websiteHistorySdts =
			await LogicMonitorClient.GetWebsiteHistorySdts(350, default)
			.ConfigureAwait(false);
		websiteHistorySdts.Should().NotBeNull();
	}

	[Fact]
	public async Task AddAndDeleteADeviceSdt()
	{
		const string initialComment = "LogicMonitor.Api unit tests - AddAndDeleteADeviceSdt initial comment";
		var deviceId = WindowsDeviceId;
		var sdtCreationDto = new DeviceScheduledDownTimeCreationDto(deviceId)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient
			.CreateAsync(sdtCreationDto, CancellationToken.None)
			.ConfigureAwait(false);
		createdSdt.Comment.Should().Be(initialComment);
		createdSdt.DeviceId.Should().Be(deviceId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await LogicMonitorClient
			.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken.None)
			.ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(initialComment);
		refetchSdt.DeviceId.Should().Be(deviceId);

		// Update
		const string newComment = "LogicMonitor.Api unit tests - AddAndDeleteADeviceSdt new comment";
		createdSdt.Comment = newComment;
		await LogicMonitorClient
			.PutStringIdentifiedItemAsync(createdSdt, CancellationToken.None)
			.ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await LogicMonitorClient
			.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken.None)
			.ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(newComment);
		refetchSdt.DeviceId.Should().Be(deviceId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}, CancellationToken.None).ConfigureAwait(false);
		scheduledDownTimes.Should().NotBeNullOrEmpty();

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(sdt.Id, CancellationToken.None).ConfigureAwait(false);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.DeviceId.Should().Be(sdt.DeviceId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}

		// Delete
		await LogicMonitorClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: CancellationToken.None).ConfigureAwait(false);
	}

	[Fact]
	public async Task AddAndDeleteAResourceGroupSdt()
	{
		const string initialComment = "LogicMonitor.Api unit tests - AddAndDeleteAResourceGroupSdt initial comment";
		const int resourceGroupId = 1; // The root
		var sdtCreationDto = new ResourceGroupScheduledDownTimeCreationDto(resourceGroupId)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient.CreateAsync(sdtCreationDto, CancellationToken.None).ConfigureAwait(false);
		createdSdt.Comment.Should().Be(initialComment);
		createdSdt.DeviceGroupId.Should().Be(resourceGroupId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken.None).ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(initialComment);
		refetchSdt.DeviceGroupId.Should().Be(resourceGroupId);

		// Update
		const string newComment = "LogicMonitor.Api unit tests - AddAndDeleteAResourceGroupSdt new comment";
		createdSdt.Comment = newComment;
		await LogicMonitorClient.PutStringIdentifiedItemAsync(createdSdt, CancellationToken.None).ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken.None).ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(newComment);
		refetchSdt.DeviceGroupId.Should().Be(resourceGroupId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceGroupSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}, CancellationToken.None).ConfigureAwait(false);
		scheduledDownTimes.Should().NotBeNullOrEmpty();

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(sdt.Id, CancellationToken.None).ConfigureAwait(false);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.DeviceGroupId.Should().Be(sdt.DeviceGroupId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}

		// Delete
		await LogicMonitorClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: CancellationToken.None).ConfigureAwait(false);
	}

	[Fact]
	public async Task AddAndDeleteACollectorSdt()
	{
		var portalClient = LogicMonitorClient;
		var collector = (await portalClient
			.GetAllAsync(new Filter<Collector> { Take = 1 }, CancellationToken.None)
			.ConfigureAwait(false))
			.SingleOrDefault();
		collector.Should().NotBeNull();
		const string initialComment = "LogicMonitor.Api unit tests - AddAndDeleteACollectorSdt initial comment";
		var collectorId = collector!.Id;
		var sdtCreationDto = new CollectorScheduledDownTimeCreationDto(collectorId)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await portalClient.CreateAsync(sdtCreationDto, CancellationToken.None).ConfigureAwait(false);
		createdSdt.Comment.Should().Be(initialComment);
		createdSdt.CollectorId.Should().Be(collectorId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken.None).ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(initialComment);
		refetchSdt.CollectorId.Should().Be(collectorId);

		// Update
		const string newComment = "LogicMonitor.Api unit tests - AddAndDeleteACollectorSdt new comment";
		createdSdt.Comment = newComment;
		await portalClient.PutStringIdentifiedItemAsync(createdSdt, CancellationToken.None).ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken.None).ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(newComment);
		refetchSdt.CollectorId.Should().Be(collectorId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "CollectorSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}, CancellationToken.None).ConfigureAwait(false);
		scheduledDownTimes.Should().NotBeNull();
		scheduledDownTimes.Should().NotBeNullOrEmpty();

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id, CancellationToken.None).ConfigureAwait(false);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.DeviceId.Should().Be(sdt.DeviceId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}

		// Delete
		await portalClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: CancellationToken.None).ConfigureAwait(false);
	}

	[Fact]
	public async Task GetScheduledDownTimesFilteredByDevice()
	{

		var allScheduledDownTimes = await LogicMonitorClient.GetAllAsync<ScheduledDownTime>(CancellationToken.None)
			.ConfigureAwait(false);

		var deviceScheduledDownTime = allScheduledDownTimes.Find(sdt => sdt.Type == ScheduledDownTimeType.Resource);
		deviceScheduledDownTime.Should().NotBeNull();
		var deviceId = deviceScheduledDownTime!.DeviceId;
		deviceId.Should().NotBeNull();

		var filteredScheduledDownTimes = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(Type), "DeviceSDT"),
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.DeviceId), deviceId!)
				}
		}, CancellationToken.None
		).ConfigureAwait(false);
		filteredScheduledDownTimes.Should().NotBeNull();

		// Get them all individually
		foreach (var sdt in filteredScheduledDownTimes)
		{
			var refetchedSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(sdt.Id, CancellationToken.None).ConfigureAwait(false);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.DeviceId.Should().Be(deviceId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}
	}

	[Fact]
	public async Task CreatePingDataSourceSdtOnEmptyDeviceGroup()
	{
		const string deviceGroupName = "CreatePingDataSourceSdtOnEmptyDeviceGroupUnitTest";

		// Ensure DeviceGroup is NOT present
		var resourceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(deviceGroupName, CancellationToken.None)
			.ConfigureAwait(false);

		if (resourceGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(resourceGroup, cancellationToken: CancellationToken.None)
				.ConfigureAwait(false);
		}

		// Create DeviceGroup
		resourceGroup = await LogicMonitorClient
			.CreateAsync(new DeviceGroupCreationDto
			{
				ParentId = "1",
				Name = deviceGroupName
			}, CancellationToken.None)
			.ConfigureAwait(false);

		// Get ping DataSource
		var dataSource = (await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource>
			{
				FilterItems = new List<FilterItem<DataSource>>
				{
						new Eq<DataSource>(nameof(DataSource.Name), "Ping")
				}
			}, CancellationToken.None)
			.ConfigureAwait(false))
			.SingleOrDefault();
		dataSource.Should().NotBeNull();

		// Create Scheduled Downtime
		var sdtCreationDto = new ResourceGroupScheduledDownTimeCreationDto(resourceGroup.Id)
		{
			Comment = "Created by Unit Test",
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime,
			DataSourceId = dataSource!.Id,
			DataSourceName = dataSource.Name
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient
			.CreateAsync(sdtCreationDto, CancellationToken.None)
			.ConfigureAwait(false);
		createdSdt.Should().NotBeNull();

		// Clean up
		await LogicMonitorClient
			.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);

		// Remove the device group
		await LogicMonitorClient
			.DeleteAsync(resourceGroup, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task CreatePingDataSourceInstanceSdtOnEmptyDeviceGroup()
	{
		// Get ping DataSource
		var dataSource = (await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource>
			{
				FilterItems = new List<FilterItem<DataSource>>
				{
						new Eq<DataSource>(nameof(DataSource.Name), "Ping")
				}
			}, CancellationToken.None)
			.ConfigureAwait(false))
			.Single();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken.None)
			.ConfigureAwait(false);

		var deviceDataSourceInstance = (await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false))
			.Single();

		// Create Scheduled Downtime
		var sdtCreationDto = new DeviceDataSourceInstanceScheduledDownTimeCreationDto(WindowsDeviceId, deviceDataSourceInstance.Id)
		{
			Comment = "Created by Unit Test",
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient
			.CreateAsync(sdtCreationDto, CancellationToken.None)
			.ConfigureAwait(false);
		createdSdt.Should().NotBeNull();

		// Clean up
		await LogicMonitorClient
			.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
	}
}
