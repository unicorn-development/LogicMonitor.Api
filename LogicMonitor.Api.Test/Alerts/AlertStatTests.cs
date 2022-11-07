namespace LogicMonitor.Api.Test.Alerts;

public class AlertStatTests : TestWithOutput
{
	public AlertStatTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAlertStat()
	{
		var alertStat = await LogicMonitorClient.GetAsync<AlertStat>(CancellationToken.None).ConfigureAwait(false);
		alertStat.Should().NotBeNull();
	}
}
