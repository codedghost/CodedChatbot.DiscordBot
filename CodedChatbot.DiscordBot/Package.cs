using System;
using System.Threading.Tasks;
using CodedChatbot.DiscordBot.Core;
using CodedChatbot.DiscordBot.Interfaces.Core;
using CodedChatbot.DiscordBot.Interfaces.Services;
using CodedChatbot.DiscordBot.Services;
using CodedGhost.Config;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Secrets;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace CodedChatbot.DiscordBot;

public static class Package
{
    public static async Task<ISecretService> AddChatbotServices(this IServiceCollection services)
    {
        var configService = new ConfigService();

        var appId = configService.Get<string>("KeyVaultAppId");
        var thumbprint = configService.Get<string>("KeyVaultCertThumbprint");
        var url = configService.Get<string>("KeyVaultBaseUrl");
        var tenantId = configService.Get<string>("ActiveDirectoryTenantId");

        var secretService = new AzureKeyVaultService(appId, thumbprint, url, tenantId);
        await secretService.Initialize();

        services.AddSingleton<IConfigService>(configService);
        services.AddSingleton<ISecretService>(secretService);

        services.AddSingleton<ILoggingService, LoggingService>();

        return secretService;
    }

    public static async Task<IServiceCollection> AddConfiguredDiscordClient(
        this IServiceCollection services,
        ISecretService secretService,
        IServiceProvider serviceProvider)
    {
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All
        };

        var discordSocketClient = new DiscordSocketClient(config);

        var loggingService = serviceProvider.GetService<ILoggingService>();

        if (loggingService == null)
        {
            return services;
        }

        discordSocketClient.Log += loggingService.Log;

        var token = secretService.GetSecret<string>("DiscordToken");

        await discordSocketClient.LoginAsync(TokenType.Bot, token);

        await discordSocketClient.StartAsync();

        services.AddSingleton<DiscordSocketClient>(discordSocketClient);

        var commandServiceConfig = new CommandServiceConfig
        {
            CaseSensitiveCommands = false,
            DefaultRunMode = RunMode.Async,
            IgnoreExtraArgs = false,
            LogLevel = LogSeverity.Verbose,
            QuotationMarkAliasMap =
            {
                {'\'', '\''}
            },
            SeparatorChar = ' ',
            ThrowOnError = true
        };
        var commandService = new CommandService(commandServiceConfig);

        services.AddSingleton(commandService);

        services.AddSingleton<ICommandHandlerService, CommandHandlerService>();

        return services;
    }

    public static IServiceCollection AddDiscordBotServices(
        this IServiceCollection services)
    {

        return services;
    }
}