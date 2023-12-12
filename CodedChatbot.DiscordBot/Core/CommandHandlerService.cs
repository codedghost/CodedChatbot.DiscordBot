using System;
using System.Reflection;
using System.Threading.Tasks;
using CodedChatbot.DiscordBot.Interfaces.Core;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace CodedChatbot.DiscordBot.Core;

public class CommandHandlerService : ICommandHandlerService
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerService(
        DiscordSocketClient client,
        CommandService commandService,
        IServiceProvider serviceProvider)
    {
        _client = client;
        _commandService = commandService;
        _serviceProvider = serviceProvider;
    }

    public async Task InstallCommandsAsync()
    {
        _client.MessageReceived += HandleCommandAsync;

        await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        var message = messageParam as SocketUserMessage;
        if (message == null)
        {
            return;
        }

        int argPos = 0;

        if (message.Content.Contains("silly sausage"))
        {
            await message.ReplyAsync("THOU SHALT NOT SPEAK THE FORBIDDEN WORDS");
        }

        if (!message.HasCharPrefix('!', ref argPos) ||
            message.Author.IsBot)
        {
            return;
        }

        var context = new SocketCommandContext(_client, message);

        await _commandService.ExecuteAsync(
            context,
            argPos,
            _serviceProvider
        );
    }
}