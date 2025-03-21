using Microsoft.Extensions.Logging;

namespace CFlat.Core.Extensions;

/// <summary>
/// A collection of extensions for logging, originally developed
/// to help log different FMOD results.
/// </summary>
public static class LoggingExtensions
{
	/// <summary>
	/// Logs an informational message if the <paramref name="result"/> was
	/// <see cref="FMOD.RESULT.OK"/>. Logs an error message containing the
	/// <paramref cref="result"/> otherwise.
	/// </summary>
	/// <param name="logger">
	/// The logger this will be called from.
	/// </param>
	/// <param name="result">
	/// The latest <see cref="FMOD.RESULT"/>.
	/// </param>
	/// <param name="message">
	/// An optional message to include in the logging statement.
	/// </param>
	public static void LogAudioResult(this ILogger logger, FMOD.RESULT result, string? message = null)
	{
		// if the result was OK and we don't have a message, don't log anything.
		if (result == FMOD.RESULT.OK && string.IsNullOrEmpty(message))
		{
			return;
		}

		var logMessage = string.Join(" :: ", message, result);
		if (result == FMOD.RESULT.OK)
		{
			logger.LogInformation(logMessage);
			return;
		}
		logger.LogError(logMessage);
	}
}
