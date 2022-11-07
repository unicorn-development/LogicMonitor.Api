namespace LogicMonitor.Api.Test.Settings;

public class BackupTests : TestWithOutput
{
	public BackupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task Backup_ExceptLongRunningOnes()
	{
		var configurationBackup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(true)
		{
			DataSources = false,
			Logs = false
		}, CancellationToken.None).ConfigureAwait(false);
		configurationBackup.Should().NotBeNull();
	}

	[Fact]
	public async Task Backup_Users()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { Users = true }, CancellationToken.None).ConfigureAwait(false);
		backup.Should().NotBeNull();
		backup.RoleGroups.Should().NotBeNull();
		backup.Roles.Should().NotBeNull();
		backup.UserGroups.Should().NotBeNull();
		backup.Users.Should().NotBeNull();

		var roleGroup = backup.RoleGroups[0];
		roleGroup.Should().NotBeNull();

		var role = backup.Roles[0];
		role.Should().NotBeNull();

		var userGroup = backup.UserGroups[0];
		userGroup.Should().NotBeNull();

		var user = backup.Users[0];
		user.Should().NotBeNull();

		// CreatedBy is populated
		user.CreatedBy.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Backup_Alerting()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { Alerting = true }, CancellationToken.None).ConfigureAwait(false);

		backup.AlertRules.Should().NotBeNullOrEmpty();

		backup.EscalationChains.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Backup_Integrations()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { Integrations = true }, CancellationToken.None).ConfigureAwait(false);

		backup.Integrations.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Backup_Dashboards()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { Dashboards = true }, CancellationToken.None).ConfigureAwait(false);

		backup.Dashboards.Should().NotBeNullOrEmpty();
		backup.Widgets.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Backup_AccountSettings()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { AccountSettings = true }, CancellationToken.None).ConfigureAwait(false);

		backup.CompanyLogo.Should().NotBeNull();
	}

	[Fact]
	public async Task Backup_AppliesToFunctions()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { AppliesToFunctions = true }, CancellationToken.None).ConfigureAwait(false);

		backup.AppliesToFunctions.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Backup_ConfigSources()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { ConfigSources = true }, CancellationToken.None).ConfigureAwait(false);

		backup.ConfigSources.Should().NotBeNullOrEmpty();
	}


	[Fact]
	public async Task Backup_ScheduledDownTimes()
	{
		var fileInfo = new FileInfo(Path.GetTempFileName());
		try
		{
			// Backup this specific item
			var backup = await LogicMonitorClient
				.BackupAsync(new ConfigurationBackupSpecification(false)
				{
					ScheduledDownTimes = true,
					GzipFileInfo = fileInfo
				}, CancellationToken.None)
				.ConfigureAwait(false);
			fileInfo.Exists.Should().BeTrue();
			backup.ScheduledDownTimes.Should().NotBeNullOrEmpty();

			// Load back from disk
			var reloadedBackup = await LogicMonitorClient
				.LoadBackupAsync(fileInfo, CancellationToken.None)
				.ConfigureAwait(false);

			reloadedBackup.Should().NotBeNull();
			reloadedBackup.Should().BeEquivalentTo(backup);
		}
		finally
		{
			fileInfo.Delete();
			fileInfo.Exists.Should().BeFalse();
		}
	}

	//[Fact(Skip = "Takes too long")]
	//public async Task Backup_DataSources()
	//{
	//	var backup = await DefaultPortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { DataSources = true }).ConfigureAwait(false);

	//	backup.DataSources.Should().NotBeNull();
	//	backup.DataSources.Should().NotBeNullOrEmpty();
	//}

	[Fact]
	public async Task Backup_EventSources()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { EventSources = true }, CancellationToken.None).ConfigureAwait(false);

		backup.EventSources.Should().NotBeNull();
		backup.EventSources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Backup_JobMonitors()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { JobMonitors = true }, CancellationToken.None).ConfigureAwait(false);

		backup.JobMonitors.Should().NotBeNull();
		backup.JobMonitors.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Backup_PropertySources()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { PropertySources = true }, CancellationToken.None).ConfigureAwait(false);

		backup.Should().NotBeNull();
		backup.PropertySources.Should().NotBeNull();
		backup.PropertySources.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task SdtBackup()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false)
		{
			ScheduledDownTimes = true
		}, CancellationToken.None).ConfigureAwait(false);

		backup.Should().NotBeNull();
		backup.ScheduledDownTimes.Should().NotBeNull();
		backup.ScheduledDownTimes.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Backup_SnmpSysOidMaps()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { SnmpSysOidMaps = true }, CancellationToken.None).ConfigureAwait(false);

		backup.SnmpSysOidMaps.Should().NotBeNull();
		backup.SnmpSysOidMaps.Should().NotBeNullOrEmpty();
	}
}
