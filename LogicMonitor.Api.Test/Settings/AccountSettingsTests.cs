namespace LogicMonitor.Api.Test.Settings;

public class AccountSettingsTests : TestWithOutput
{
	public AccountSettingsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task Get()
	{
		var accountSettings = await LogicMonitorClient.GetAsync<AccountSettings>(CancellationToken.None).ConfigureAwait(false);
		accountSettings.Should().NotBeNull();
		(accountSettings.DeviceCount > 0).Should().BeTrue();
	}

	[Fact]
	public async Task GetBillingInformation()
	{
		if (!AccountHasBillingInformation)
		{
			// Our test account does not have billing information - we can't test this.
			return;
		}

		var billingInformation = await LogicMonitorClient.GetAsync<BillingInformation>(CancellationToken.None).ConfigureAwait(false);

		billingInformation.Should().NotBeNull();
	}
}
