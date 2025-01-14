namespace LogicMonitor.Api.Users;

/// <summary>
/// View permissions
/// </summary>
[DataContract]
public class ViewPermission
{
	/// <summary>
	/// Alerts view permission
	/// </summary>
	[DataMember(Name = "Alerts")]
	public bool Alerts { get; set; }

	/// <summary>
	/// BizService view permission
	/// </summary>
	[DataMember(Name = "BizService")]
	public bool BizService { get; set; }

	/// <summary>
	/// Dashboards view permission
	/// </summary>
	[DataMember(Name = "Dashboards")]
	public bool Dashboards { get; set; }

	/// <summary>
	/// Devices view permission
	/// </summary>
	[DataMember(Name = "Resources")]
	public bool Devices { get; set; }

	/// <summary>
	/// Logs view permission
	/// </summary>
	[DataMember(Name = "Logs")]
	public bool Logs { get; set; }

	/// <summary>
	/// Maps view permission
	/// </summary>
	[DataMember(Name = "Maps")]
	public bool Maps { get; set; }

	/// <summary>
	/// Whether to use the new UI
	/// </summary>
	[DataMember(Name = "NewUI")]
	public bool? NewUi { get; set; }

	/// <summary>
	/// Whether to use the new UI
	/// </summary>
	[DataMember(Name = "OnlyNewUI")]
	public bool? OnlyNewUi { get; set; }

	/// <summary>
	/// Recommendations view permission
	/// </summary>
	[DataMember(Name = "Recommendations")]
	public bool Recommendations { get; set; }

	/// <summary>
	/// Reports view permission
	/// </summary>
	[DataMember(Name = "Reports")]
	public bool Reports { get; set; }

	/// <summary>
	/// Settings view permission
	/// </summary>
	[DataMember(Name = "Settings")]
	public bool Settings { get; set; }

	/// <summary>
	/// Websites view permission
	/// </summary>
	[DataMember(Name = "Websites")]
	public bool Websites { get; set; }

	/// <summary>
	/// Traces view permission
	/// </summary>
	[DataMember(Name = "Traces")]
	public bool Traces { get; set; }

	/// <inheritdoc />
	public override string ToString() =>
		$"{nameof(Dashboards)}={Dashboards};" +
		$"{nameof(Recommendations)}={Recommendations};" +
		$"{nameof(Reports)}={Reports};" +
		$"{nameof(Websites)}={Websites};" +
		$"{nameof(Settings)}={Settings};" +
		$"{nameof(Devices)}={Devices};" +
		$"{nameof(Alerts)}={Alerts};" +
		$"{nameof(Maps)}={Maps};" +
		$"{nameof(Logs)}={Logs};" +
		$"{nameof(Traces)}={Traces};";
}
