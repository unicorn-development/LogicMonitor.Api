﻿namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A disabled group
/// </summary>
[DataContract]
public class DisabledGroup : IdentifiedItem
{
	/// <summary>
	///    The LogicMonitor Display Name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string Name { get; set; }

	/// <summary>
	///    The LogicMonitor Display Name
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	///    The user permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermissionValues UserPermission { get; set; }
}
