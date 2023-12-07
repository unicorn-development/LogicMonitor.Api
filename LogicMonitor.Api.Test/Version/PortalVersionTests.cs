namespace LogicMonitor.Api.Test.Settings;

public class PortalVersionTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetPortalVersion()
	{
		var portalVersion = await LogicMonitorClient
			.GetVersionAsync(default)
			.ConfigureAwait(true);
		portalVersion.Version.Should().NotBeNull();
		portalVersion.Version.Module.Should().NotBeNull();
		portalVersion.Extra.Should().NotBeNull();
		portalVersion.Hash.Should().NotBeNull();
		portalVersion.BuildAt.Should().NotBeNull();
		portalVersion.Branch.Should().NotBeNull();
		portalVersion.ResultKey.Should().NotBeNull();
	}

	[Fact]
	public async Task GetPortalVersionStatic()
	{
		var portalVersion = await LogicMonitorClient
			.GetVersionAsync("panoramicdata", default)
			.ConfigureAwait(true);
		portalVersion.Version.Should().NotBeNull();
		((object)portalVersion.Version.Module).Should().NotBeNull();
		portalVersion.Extra.Should().NotBeNull();
		portalVersion.Hash.Should().NotBeNull();
		portalVersion.BuildAt.Should().NotBeNull();
		portalVersion.Branch.Should().NotBeNull();
		portalVersion.ResultKey.Should().NotBeNull();
	}
}
