namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Group Netflow widget
/// </summary>
[DataContract]
public class GroupNetflowWidget : Widget
{
	/// <summary>
	///     The data type
	/// </summary>
	[DataMember(Name = "dataType")]
	public string DataType { get; set; } = string.Empty;

	/// <summary>
	///     The QoS type
	/// </summary>
	[DataMember(Name = "qosType")]
	public string QosType { get; set; } = string.Empty;

	/// <summary>
	///     The device group Id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int DeviceGroupId { get; set; }

	/// <summary>
	///     The device group name
	/// </summary>
	[DataMember(Name = "deviceGroupName")]
	public string DeviceGroupName { get; set; } = string.Empty;

	/// <summary>
	///     The row filters
	/// </summary>
	[DataMember(Name = "rowFilters")]
	public string RowFilters { get; set; } = string.Empty;
}
