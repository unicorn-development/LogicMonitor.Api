namespace LogicMonitor.Api.Test.Websites;

public class WebsiteGroupTests : TestWithOutput
{
	public WebsiteGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void SetWebsiteGroupCustomProperty()
	{
		// Create a device group for testing purposes
		const string testWebsiteGroupName = "Property Test Device Group";
		var existingWebsiteGroup = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync(testWebsiteGroupName)
			.ConfigureAwait(false);
		if (existingWebsiteGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(existingWebsiteGroup)
				.ConfigureAwait(false);
		}

		var deviceGroup = await LogicMonitorClient.CreateAsync(new WebsiteGroupCreationDto
		{
			ParentId = "1",
			Name = testWebsiteGroupName
		}).ConfigureAwait(false);

		const string propertyName = "blah";
		const string value1 = "test1";
		const string value2 = "test2";

		// Set it to an expected value
		await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1).ConfigureAwait(false);
		var deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
		var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Set it to a different value
		await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Set it to null (delete it)
		await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);

		// This should fail as there is nothing to delete
		var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false)).ConfigureAwait(false);
		deletionException.Should().BeOfType<LogicMonitorApiException>();

		var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Update).ConfigureAwait(false)).ConfigureAwait(false);
		updateException.Should().BeOfType<InvalidOperationException>();

		var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Create).ConfigureAwait(false)).ConfigureAwait(false);
		createNullException.Should().BeOfType<InvalidOperationException>();

		// Create one without checking
		await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value1, SetPropertyMode.Create).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
		actual.Should().Be(1);

		// Update one without checking
		await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, value2, SetPropertyMode.Update).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
		actual.Should().Be(1);

		// Delete one without checking
		await LogicMonitorClient.SetWebsiteGroupCustomPropertyAsync(deviceGroup.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false);
		deviceProperties = await LogicMonitorClient.GetWebsiteGroupPropertiesAsync(deviceGroup.Id).ConfigureAwait(false);
		actual = deviceProperties.Count(dp => dp.Name == propertyName);
		actual.Should().Be(0);
	}

	[Fact]
	public async void WebsiteGroupCreationShouldSetProperties()
	{
		// Create it
		var websiteGroup = await LogicMonitorClient.CreateAsync(new WebsiteGroupCreationDto
		{
			Name = "Test Name",
			Description = "Test Description",
			IsAlertingDisabled = false,
			IsMonitoringDisabled = false,
			ParentGroupFullPath = "",
			ParentId = "1",
			Properties = new List<Property>
					{new Property {Name = "name", Value = "value"}},
			//TestLocation = new TestLocation { All = false, SmgIds = new List<int> { 3, 4 } }
		}).ConfigureAwait(false);

		try
		{
			// Check everything was set correctly
			websiteGroup.Name.Should().Be("Test Name");
			websiteGroup.Description.Should().Be("Test Description");
			websiteGroup.DisableAlerting.Should().BeFalse();
			websiteGroup.StopMonitoring.Should().BeFalse();
			websiteGroup.FullPath.Should().Be("Test Name");
			websiteGroup.ParentId.Should().Be(1);
			//websiteGroup.TestLocation.Should().BeEquivalentTo(new TestLocation { All = false, SmgIds = new List<int> { 3, 4 } });
			websiteGroup.CustomProperties.Should().BeEquivalentTo(new List<Property> { new Property { Name = "name", Value = "value" } });
		}
		finally
		{
			// Delete it
			await LogicMonitorClient.DeleteAsync(websiteGroup).ConfigureAwait(false);
		}
	}

	[Fact]
	public async void GetWebsiteGroupByFullPath()
	{
		var websiteGroup0 = await LogicMonitorClient.GetWebsiteGroupByFullPathAsync(string.Empty).ConfigureAwait(false);
		websiteGroup0.Should().NotBeNull();
		websiteGroup0.FullPath.Should().Be(string.Empty);
		var websiteGroup1 = await LogicMonitorClient.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath).ConfigureAwait(false);
		websiteGroup1.Should().NotBeNull();
		var websiteGroup2 = await LogicMonitorClient.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath).ConfigureAwait(false);
		websiteGroup2.Should().NotBeNull();
		websiteGroup0.Id.Should().NotBe(websiteGroup2.Id);
	}

	[Fact]
	public async void GetWebsiteGroupById()
	{
		var websiteGroup = await LogicMonitorClient.GetAsync<WebsiteGroup>(1).ConfigureAwait(false);

		websiteGroup.Should().NotBeNull();
		websiteGroup.ChildWebsiteGroups.Should().NotBeNullOrEmpty();
		websiteGroup.ParentId.Should().Be(0);
		websiteGroup.Id.Should().Be(1);
		string.IsNullOrWhiteSpace(websiteGroup.Name).Should().BeFalse();
		(websiteGroup.FullPath is null).Should().BeFalse();
	}
}
