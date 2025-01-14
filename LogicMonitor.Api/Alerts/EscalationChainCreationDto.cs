﻿namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An Escalation chain creation DTO
/// </summary>
public class EscalationChainCreationDto : CreationDto<EscalationChain>
{
	/// <summary>
	///    The LogicMonitor Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The LogicMonitor Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Whether throttling is enabled
	/// </summary>
	[DataMember(Name = "enableThrottling")]
	public bool EnableThrottling { get; set; }

	/// <summary>
	/// The throttling period in seconds
	/// </summary>
	[DataMember(Name = "throttlingPeriod")]
	public int ThrottlingPeriodMinutes { get; set; }

	/// <summary>
	/// The alert count for throttling
	/// </summary>
	[DataMember(Name = "throttlingAlerts")]
	public int ThrottlingAlertCount { get; set; }

	/// <summary>
	/// Whether in alerting
	/// </summary>
	[DataMember(Name = "inAlerting")]
	public bool InAlerting { get; set; }

	/// <summary>
	/// The cc destinations
	/// </summary>
	[DataMember(Name = "ccdestination")]
	public List<Destination> CcDestination { get; set; } = [];

	/// <summary>
	/// The CC destinations
	/// </summary>
	[DataMember(Name = "ccDestinations")]
	public List<Destination> CcDestinations { get; set; } = [];

	/// <summary>
	/// The destinations
	/// </summary>
	[DataMember(Name = "destination")]
	public List<Destination> Destination { get; set; } = [];

	/// <summary>
	/// The destinations
	/// </summary>
	[DataMember(Name = "destinations")]
	public List<Destination> Destinations { get; set; } = [];
}
