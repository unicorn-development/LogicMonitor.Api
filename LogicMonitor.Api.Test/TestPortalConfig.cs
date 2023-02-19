namespace LogicMonitor.Api.Test;

internal class TestPortalConfig
{
	internal TestPortalConfig(ILogger logger)
	{
		var location = typeof(TestPortalConfig).GetTypeInfo().Assembly.Location;
		var dirPath = Path.Combine(Path.GetDirectoryName(location), "../../../..");
		var builder = new ConfigurationBuilder()
			.SetBasePath(dirPath)
			.AddUserSecrets<TestPortalConfig>();
		Configuration = builder.Build();

		LogicMonitorClient = new LogicMonitorClient(
			new LogicMonitorClientOptions
			{
				Account = Configuration["Config:Account"],
				AccessId = Configuration["Config:AccessId"],
				AccessKey = Configuration["Config:AccessKey"],
				Logger = logger
			}
		)
		{
			StrictPagingTotalChecking = true
		};
		SnmpDeviceId = int.Parse(Configuration["Config:SnmpDeviceId"]);
		NetflowDeviceId = int.Parse(Configuration["Config:NetflowDeviceId"]);
		WindowsDeviceId = int.Parse(Configuration["Config:WindowsDeviceId"]);
		WindowsDeviceLargeDeviceDataSourceId = int.Parse(Configuration["Config:WindowsDeviceLargeDeviceDataSourceId"]);
		ServiceDeviceId = int.Parse(Configuration["Config:ServiceDeviceId"]);
		CollectorId = int.Parse(Configuration["Config:CollectorId"]);
		SDTResourceGroupId = int.Parse(Configuration["Config:SDTResourceGroupId"]);
		WebsiteGroupFullPath = Configuration["Config:WebsiteGroupFullPath"];
		DeviceGroupFullPath = Configuration["Config:DeviceGroupFullPath"];
		ResourceGroupFullPath = Configuration["Config:ResourceGroupFullPath"];
		AllWidgetsDashboardId = int.Parse(Configuration["Config:AllWidgetsDashboardId"]);
		AccountHasBillingInformation = bool.Parse(Configuration["Config:AccountHasBillingInformation"]);
		WebsiteName = Configuration["Config:WebsiteName"];
		AlertRuleName = Configuration["Config:AlertRuleName"];
	}

	public static IConfigurationRoot Configuration { get; set; }

	public LogicMonitorClient LogicMonitorClient { get; }

	public int NetflowDeviceId { get; }

	public int SnmpDeviceId { get; }

	public int WindowsDeviceId { get; }

	public int WindowsDeviceLargeDeviceDataSourceId { get; }

	public int ServiceDeviceId { get; }

	public bool AccountHasBillingInformation { get; }

	public int AllWidgetsDashboardId { get; }

	public string WebsiteGroupFullPath { get; }

	public string DeviceGroupFullPath { get; }

	public string ResourceGroupFullPath { get; }

	public string WebsiteName { get; }

	internal int CollectorId { get; }

	public int SDTResourceGroupId { get; }

	public string AlertRuleName { get; }
}
