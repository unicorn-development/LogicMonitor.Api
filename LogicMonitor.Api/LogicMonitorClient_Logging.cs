namespace LogicMonitor.Api;

/// <summary>
///     LogItems
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Logs multiple items
	/// </summary>
	/// <param name="writeLogRequests">A list of log write requests</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		IEnumerable<WriteLogRequest> writeLogRequests,
		CancellationToken cancellationToken)
		=> PostAsync<IEnumerable<WriteLogRequest>, WriteLogResponse>(writeLogRequests, "log/ingest", cancellationToken);

	/// <summary>
	///     Logs a single writeLogRequest
	/// </summary>
	/// <param name="writeLogRequest">A single log write requests</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		WriteLogRequest writeLogRequest,
		CancellationToken cancellationToken)
		=> WriteLogAsync(new[] { writeLogRequest }, cancellationToken);

	/// <summary>
	///     Logs a single writeLogRequest at the specified level
	/// </summary>
	/// <param name="level"></param>
	/// <param name="deviceId">The device id</param>
	/// <param name="message">The message</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		WriteLogLevel level,
		int deviceId,
		string message,
		CancellationToken cancellationToken)
		=> WriteLogAsync(new[] { new WriteLogRequest(level, deviceId, message) }, cancellationToken);

	/// <summary>
	///     Logs a single writeLogRequest at the informational level
	/// </summary>
	/// <param name="deviceId">The device id</param>
	/// <param name="message">The message</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		int deviceId,
		string message,
		CancellationToken cancellationToken)
		=> WriteLogAsync(new[] { new WriteLogRequest(deviceId, message) }, cancellationToken);
}
