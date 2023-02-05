namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An autodiscovery method
/// </summary>
[DataContract]
public class AutoDiscoveryMethod
{
	/// <summary>
	/// The auto discovery method name values can be : ad_cim|ad_cloudwatch|ad_collector|ad_dummy|ad_ec2|ad_esx|ad_http|ad_ipmi|ad_jdbc|ad_jmx|ad_netapp|ad_pdh|ad_port|ad_script|ad_snmp|ad_wmi|ad_xen|ad_azurerediscache|ad_awsserviceregion|ad_awsec2reservedinstance|ad_awsec2reservedinstancecoverage|ad_awsecsservice|ad_awsec2scheduledevents|ad_azureserviceregion|ad_azuresubscription|ad_azurebackupjob|ad_azuresdk|ad_azurewebjob|ad_awsbillingreport|ad_awselasticache|ad_awsredshift|ad_azurebilling|ad_awslbtargetgroups|ad_gcpappengine|ad_gcpbilling|ad_awsvpntunnel|ad_gcpvpntunnel|ad_awsglobalwebacl|ad_gcplbbackendservice|ad_gcppubsubsubscription|ad_gcppubsubsnapshot|ad_azurereplicationjob|ad_azureexpressroutecircuitpeering|ad_awsapigatewaystage|ad_azureautomationaccountcertificate|ad_azurevngconnection|ad_azurewebappinstance|ad_azureappserviceenvironmentmultirolepool|ad_openmetrics|ad_awsmediaconnectoutput|ad_awsmediaconnectsource|ad_awswebaclwafv2|ad_saaso365sharepointsite|ad_awscognitoidentityproviders|ad_azureeabilling|ad_saaszoomplanusage|ad_saasstatus|ad_azuresynapse|ad_saasairbrake|ad_syntheticsselenium|ad_azurevirtualdesktopsessionhosts|ad_saaso365subscribedsku|ad_azuredimension|ad_azurecostmanagementdimensions|ad_saaso365servicehealth|ad_saaso365mailbox|ad_azurenetappvolumes|ad_azureloganalyticsworkspaces|ad_saaszoomstatus|ad_saassalesforcelicense|ad_saaszoomroom|ad_saaswebexlicenseusage|ad_azureloganalyticsreplicationjob|ad_paasjsonpath
	/// </summary>
	[DataMember(Name = "name", IsRequired = true)]
	public string Name { get; set; } = null!;
}
