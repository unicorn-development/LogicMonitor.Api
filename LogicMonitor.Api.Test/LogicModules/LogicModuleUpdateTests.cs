namespace LogicMonitor.Api.Test.LogicModules;

public class LogicModuleUpdateTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{

	/// <summary>
	/// Get DataSource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleDataSourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var dataSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.DataSource, version.Version.Major, default)
				.ConfigureAwait(true);

		dataSourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get EventSource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleEventSourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var eventSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.EventSource, version.Version.Major, default)
				.ConfigureAwait(true);

		eventSourceUpdates.Items.Should().NotBeNull();
	}

	/// <summary>
	/// Get ConfigSource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleConfigSourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var configSourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.ConfigSource, version.Version.Major, default)
				.ConfigureAwait(true);

		configSourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get PropertySource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModulePropertySourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var propertySourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.PropertySource, version.Version.Major, default)
				.ConfigureAwait(true);

		propertySourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get TopologySource updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleTopologySourceUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var topologySourceUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.TopologySource, version.Version.Major, default)
				.ConfigureAwait(true);

		topologySourceUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get Job Monitor updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleJobMonitorUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		_ = await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.PropertySource, version.Version.Major, default)
				.ConfigureAwait(true);

		//jobMonitorUpdates.Items.Should().NotBeNullOrEmpty();	// Usually none
	}

	/// <summary>
	/// Get AppliesTo Function updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleAppliesToUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		_ = await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.AppliesToFunction, version.Version.Major, default)
				.ConfigureAwait(true);

		//appliesToUpdates.Items.Should().NotBeNullOrEmpty();	// Usually none
	}

	/// <summary>
	/// Get SnmpSysOID updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetLogicModuleSnmpSysOidUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var snmpSysOidUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.SnmpSysOIDMap, version.Version.Major, default)
				.ConfigureAwait(true);

		snmpSysOidUpdates.Items.Should().NotBeNullOrEmpty();
	}

	/// <summary>
	/// Get ALL LogicModule updates
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task GetAllLogicModuleUpdates()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var allUpdates =
			await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.All, version.Version.Major, default)
				.ConfigureAwait(true);

		allUpdates.Total.Should().BePositive();
	}

	/// <summary>
	/// Find one unaudited data source update and mark as audited
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task AuditDataSource()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var dataSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.DataSource, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedNotInUse)
			.ToList();

		if (dataSourceUpdates.Count > 0)
		{
			var dataSourceToAudit = dataSourceUpdates[0];
			_ = await LogicMonitorClient.AuditDataSourceAsync(
					dataSourceToAudit.LocalId,
					dataSourceToAudit.Version,
					default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one unaudited event source update and mark as audited
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task AuditEventSource()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var eventSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.EventSource, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (eventSourceUpdates.Count > 0)
		{
			var eventSourceToAudit = eventSourceUpdates[0];
			_ = await LogicMonitorClient.AuditEventSourceAsync(
					eventSourceToAudit.LocalId,
					eventSourceToAudit.Version,
					default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one unaudited config source update and mark as audited
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task AuditConfigSource()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var configSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.ConfigSource, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (configSourceUpdates.Count > 0)
		{
			var configSourceToAudit = configSourceUpdates[0];
			_ =
				await LogicMonitorClient.AuditConfigSourceAsync(
					configSourceToAudit.LocalId,
					configSourceToAudit.Version,
					default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one unaudited property source update and mark as audited
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task AuditPropertySource()
	{
		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var propertySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(LogicModuleType.PropertySource, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
			.ToList();

		if (propertySourceUpdates.Count > 0)
		{
			var propertySourceToAudit = propertySourceUpdates[0];
			_ = await LogicMonitorClient.AuditPropertySourceAsync(
					propertySourceToAudit.LocalId,
					propertySourceToAudit.Version,
					default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one updated Data Source and import
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task ImportDataSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.DataSource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var dataSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (dataSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						dataSourceUpdates[0].Name
				],
				version.Version.Major,
				default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one updated Event Source and import
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task ImportEventSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.EventSource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var eventSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (eventSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						eventSourceUpdates[0].Name
				],
				version.Version.Major,
				default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one updated Config Source and import
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task ImportConfigSource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.ConfigSource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var configSourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (configSourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						configSourceUpdates[0].Name
				],
				version.Version.Major,
				default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one updated Property Source and import
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task ImportPropertySource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.PropertySource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var propertySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (propertySourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						propertySourceUpdates[0].Name
				],
				version.Version.Major,
				default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one updated Topology Source and import
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task ImportTopologySource()
	{
		const LogicModuleType logicModuleType = LogicModuleType.TopologySource;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var topologySourceUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (topologySourceUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						topologySourceUpdates[0].Name
				],
				version.Version.Major,
				default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one updated Job Monitor and import
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task ImportJobMonitor()
	{
		const LogicModuleType logicModuleType = LogicModuleType.JobMonitor;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var jobMonitorUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (jobMonitorUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						jobMonitorUpdates[0].Name
				],
				default,
				default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one updated AppliesToFunction and import
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task ImportAppliesToFunction()
	{
		const LogicModuleType logicModuleType = LogicModuleType.AppliesToFunction;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var appliesToFunctionUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (appliesToFunctionUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportLogicModulesAsync(
				logicModuleType,
				[
						appliesToFunctionUpdates[0].Name
				],
				default,
				default)
				.ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Find one updated SNMP SysOID Map and import
	/// </summary>
	[Fact]
	[Trait("Long Tests", "")]
	public async Task ImportSnmpSysOidMap()
	{
		const LogicModuleType logicModuleType = LogicModuleType.SnmpSysOIDMap;

		var version = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);

		var snmpSysOidMapUpdates =
			(await LogicMonitorClient
				.GetLogicModuleUpdatesAsync(logicModuleType, version.Version.Major, default)
				.ConfigureAwait(true))
			.Items
			.Where(ds =>
				ds.Category == LogicModuleUpdateCategory.New)
			.ToList();

		if (snmpSysOidMapUpdates.Count > 0)
		{
			await LogicMonitorClient
				.ImportSnmpSysOidMapAsync(
				[
						new SnmpSysOidMapImportItem
						{
							Id = snmpSysOidMapUpdates[0].LocalId,
							Oid = snmpSysOidMapUpdates[0].Name
						}
				],
				version.Version.Major,
				default)
				.ConfigureAwait(true);
		}
	}
}
