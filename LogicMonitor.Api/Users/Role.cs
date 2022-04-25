namespace LogicMonitor.Api.Users;

/// <summary>
///    A LogicMonitor Role
/// </summary>
[DataContract]
public class Role : NamedItem, IHasEndpoint
{
	/// <summary>
	///    Requires
	/// </summary>
	[DataMember(Name = "requireEULA")]
	public string RequiresEula { get; set; }

	/// <summary>
	///    Whether remote session is enabled at company level
	/// </summary>
	[DataMember(Name = "enableRemoteSessionInCompanyLevel")]
	public bool IsRemoteSessionEnabledAtCompanyLevel { get; set; }

	/// <summary>
	///    Requires two factor authentication
	/// </summary>
	[DataMember(Name = "twoFARequired")]
	public bool IsTwoFactorAuthenticationRequiredByRole { get; set; }

	/// <summary>
	///    Requires two factor authentication
	/// </summary>
	[DataMember(Name = "acctRequireTwoFA")]
	public bool IsTwoFactorAuthenticationRequiredByAccount { get; set; }

	/// <summary>
	///    A custom help label
	/// </summary>
	[DataMember(Name = "customHelpLabel")]
	public string CustomHelpLabel { get; set; } = null!;

	/// <summary>
	///    A custom help URL
	/// </summary>
	[DataMember(Name = "customHelpUrl")]
	public string CustomHelpUrl { get; set; } = null!;

	/// <summary>
	///    The associated user count
	/// </summary>
	[DataMember(Name = "associatedUserCount")]
	public string AssociatedUserCount { get; set; } = null!;

	/// <summary>
	///    The associated user count
	/// </summary>
	[DataMember(Name = "privileges")]
	public List<RolePrivilege> Privileges { get; set; } = null!;

	/// <summary>
	///    The role group id
	/// </summary>
	[DataMember(Name = "roleGroupId")]
	public int RoleGroupId { get; set; }

	/// <summary>
	///    The user permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission? UserPermission { get; set; }

	/// <summary>
	///    Whether this is a SAML role
	/// </summary>
	[DataMember(Name = "isSamlRole")]
	public bool IsSamlRole { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/roles";
}
