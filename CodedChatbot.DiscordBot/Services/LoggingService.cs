using System;
using System.Threading.Tasks;
using CodedChatbot.DiscordBot.Interfaces.Services;
using Discord;
using Microsoft.Extensions.Logging;

namespace CodedChatbot.DiscordBot.Services;

public class LoggingService : ILoggingService
{
    private readonly ILogger<Program> _logger;

    public LoggingService(ILogger<Program> logger)
    {
        _logger = logger;
    }

    public Task Log(LogMessage message)
    {
        var logLevel = message.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            _ => throw new ArgumentOutOfRangeException()
        };
            
        _logger.Log(logLevel, message.Exception, message.Message, message.Source);

        return Task.CompletedTask;
    }
}