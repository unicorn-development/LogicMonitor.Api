﻿namespace LogicMonitor.Api.ScheduledDownTimes;
/// <summary>
/// DeviceLogPipeLineResourceSDT
/// </summary>

[DataContract]
public class DeviceLogPipeLineResourceSDT
{
	/// <summary>
	/// The id of the device logPipeLineResource that the SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceLogPipeLineResourceId", IsRequired = false)]
	public int DeviceLogPipeLineResourceId { get; set; }

	/// <summary>
	/// The name of the pipe line that the SDT will apply to
	/// </summary>
	[DataMember(Name = "logPipeLineName", IsRequired = false)]
	public string? LogPipeLineName { get; set; }

	/// <summary>
	/// The id of the device associated with the pipe line that the SDT will apply to
	/// </summary>
	[DataMember(Name = "deviceId", IsRequired = false)]
	public int DeviceId { get; set; }

	/// <summary>
	/// The display name of the device associated with the logPipeLine that the SDT will apply to
	/// </summary>
	[DataMember(Name = "deviceDisplayName", IsRequired = false)]
	public string? DeviceDisplayName { get; set; }
}
