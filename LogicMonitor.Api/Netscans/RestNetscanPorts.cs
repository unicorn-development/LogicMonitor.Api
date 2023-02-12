namespace LogicMonitor.Api.Netscans;

/// <summary>
/// Netscan ports
/// </summary>
[DataContract]
public class RestNetscanPorts
{
	/// <summary>
	/// Whether or not default ports should be used
	/// </summary>
	[DataMember(Name = "isGlobalDefault", IsRequired = false)]
	public bool IsGlobalDefault { get; set; }

	/// <summary>
	/// The ports that should be used in the Netscan
	/// </summary>
	[DataMember(Name = "value", IsRequired = false)]
	public string? Value { get; set; }
}
