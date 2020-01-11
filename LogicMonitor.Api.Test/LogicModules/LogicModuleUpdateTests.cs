using LogicMonitor.Api.LogicModules;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.LogicModules
{
	public class LogicModuleUpdateTests : TestWithOutput
	{
		public LogicModuleUpdateTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		/// <summary>
		/// Get DataSource updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleDataSourceUpdates()
		{
			var dataSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.DataSource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(dataSourceUpdates.Items);
		}

		/// <summary>
		/// Get EventSource updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleEventSourceUpdates()
		{
			var eventSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.EventSource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(eventSourceUpdates.Items);
		}

		/// <summary>
		/// Get ConfigSource updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleConfigSourceUpdates()
		{
			var configSourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.ConfigSource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(configSourceUpdates.Items);
		}

		/// <summary>
		/// Get PropertySource updates
		/// </summary>
		[Fact]
		public async void GetLogicModulePropertySourceUpdates()
		{
			var propertySourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.PropertySource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(propertySourceUpdates.Items);
		}

		/// <summary>
		/// Get TopologySource updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleTopologySourceUpdates()
		{
			var topologySourceUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.TopologySource, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(topologySourceUpdates.Items);
		}

		/// <summary>
		/// Get Job Monitor updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleJobMonitorUpdates()
		{
			var _ =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.PropertySource, default)
					.ConfigureAwait(false);

			//Assert.NotEmpty(jobMonitorUpdates.Items);	// Usually none
		}

		/// <summary>
		/// Get AppliesTo Function updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleAppliesToUpdates()
		{
			var _ =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.AppliesToFunction, default)
					.ConfigureAwait(false);

			//Assert.NotEmpty(appliesToUpdates.Items);	// Usually none
		}

		/// <summary>
		/// Get SnmpSysOID updates
		/// </summary>
		[Fact]
		public async void GetLogicModuleSnmpSysOidUpdates()
		{
			var snmpSysOidUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.SnmpSysOIDMap, default)
					.ConfigureAwait(false);

			Assert.NotEmpty(snmpSysOidUpdates.Items);
		}

		/// <summary>
		/// Get ALL LogicModule updates
		/// </summary>
		[Fact]
		public async void GetAllLogicModuleUpdates()
		{
			var allUpdates =
				await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.All, default)
					.ConfigureAwait(false);

			Assert.True(allUpdates.Total > 0);
		}

		/// <summary>
		/// Find one unaudited data source update and mark as audited
		/// </summary>
		[Fact]
		public async void AuditDataSource()
		{
			var dataSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.DataSource, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
				.ToList();

			if (dataSourceUpdates.Count > 0)
			{
				var dataSourceToAudit = dataSourceUpdates[0];
				var auditedDataSource =
					await PortalClient.AuditDataSource(
						dataSourceToAudit.LocalId,
						dataSourceToAudit.Version,
						default)
					.ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Find one unaudited event source update and mark as audited
		/// </summary>
		[Fact]
		public async void AuditEventSource()
		{
			var eventSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.EventSource, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
				.ToList();

			if (eventSourceUpdates.Count > 0)
			{
				var eventSourceToAudit = eventSourceUpdates[0];
				var auditedEventSource =
					await PortalClient.AuditEventSource(
						eventSourceToAudit.LocalId,
						eventSourceToAudit.Version,
						default)
					.ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Find one unaudited config source update and mark as audited
		/// </summary>
		[Fact]
		public async void AuditConfigSource()
		{
			var configSourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.ConfigSource, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
				.ToList();

			if (configSourceUpdates.Count > 0)
			{
				var configSourceToAudit = configSourceUpdates[0];
				var auditedConfigSource =
					await PortalClient.AuditConfigSource(
						configSourceToAudit.LocalId,
						configSourceToAudit.Version,
						default)
					.ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Find one unaudited property source update and mark as audited
		/// </summary>
		[Fact]
		public async void AuditPropertySource()
		{
			var propertySourceUpdates =
				(await PortalClient
					.GetLogicModuleUpdates(LogicModuleType.PropertySource, default)
					.ConfigureAwait(false))
				.Items
				.Where(ds =>
					ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
				.ToList();

			if (propertySourceUpdates.Count > 0)
			{
				var propertySourceToAudit = propertySourceUpdates[0];
				var auditedPropertySource =
					await PortalClient.AuditPropertySource(
						propertySourceToAudit.LocalId,
						propertySourceToAudit.Version,
						default)
					.ConfigureAwait(false);
			}
		}

		///// <summary>
		///// Find one unaudited topology source update and mark as audited. LM NOT CURRENTLY SUPPORTS AUDITING A TOPOLOGY SOURCE
		///// </summary>
		//[Fact]
		//public async void AuditTopologySource()
		//{
		//	var topologySourceUpdates =
		//		(await PortalClient
		//			.GetLogicModuleUpdates(LogicModuleType.TopologySource, default)
		//			.ConfigureAwait(false))
		//		.Items
		//		.Where(ds =>
		//			ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
		//		.ToList();

		//	if (topologySourceUpdates.Count > 0)
		//	{
		//		var topologySourceToAudit = topologySourceUpdates[0];
		//		var auditedTopologySource =
		//			await PortalClient.AuditTopologySource(
		//				topologySourceToAudit.LocalId,
		//				topologySourceToAudit.Version,
		//				default)
		//			.ConfigureAwait(false);
		//	}
		//}

		///// <summary>
		///// Find one unaudited job monitor update and mark as audited. LM NOT CURRENTLY SUPPORTS AUDITING A JOB MONITOR
		///// </summary>
		//[Fact]
		//public async void AuditJobMonitor()
		//{
		//	var jobMonitorUpdates =
		//		(await PortalClient
		//			.GetLogicModuleUpdates(LogicModuleType.JobMonitor, default)
		//			.ConfigureAwait(false))
		//		.Items
		//		.Where(ds =>
		//			ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
		//		.ToList();

		//	if (jobMonitorUpdates.Count > 0)
		//	{
		//		var jobMonitorToAudit = jobMonitorUpdates[0];
		//		var auditedJobMonitor =
		//			await PortalClient.AuditTopologySource(
		//				jobMonitorToAudit.LocalId,
		//				jobMonitorToAudit.Version,
		//				default)
		//			.ConfigureAwait(false);
		//	}
		//}

		///// <summary>
		///// Find one unaudited applies to function update and mark as audited. LM NOT CURRENTLY SUPPORTS AUDITING AN APPLIESTO
		///// </summary>
		//[Fact]
		//public async void AuditAppliesToFunction()
		//{
		//	var appliesToUpdates =
		//		(await PortalClient
		//			.GetLogicModuleUpdates(LogicModuleType.AppliesToFunction, default)
		//			.ConfigureAwait(false))
		//		.Items
		//		.Where(ds =>
		//			ds.Category == LogicModuleUpdateCategory.UpdatedInUse)
		//		.ToList();

		//	if (appliesToUpdates.Count > 0)
		//	{
		//		var appliesToToAudit = appliesToUpdates[0];
		//		var auditedAppliesTo =
		//			await PortalClient.AuditAppliesToFunction(
		//				appliesToToAudit.LocalId,
		//				appliesToToAudit.Version,
		//				default)
		//			.ConfigureAwait(false);
		//	}
		//}
	}
}