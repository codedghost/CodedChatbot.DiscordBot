using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using CodedChatbot.DiscordBot.Helpers;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CodedChatbot.DiscordBot.Commands;

public class ReactCommand : ModuleBase<SocketCommandContext>
{
    private readonly DiscordSocketClient _client;

    public ReactCommand(DiscordSocketClient client)
    {
        _client = client;
    }

    [Command("react")]
    [Summary("Reacts to a message...")]
    public async Task ReactAsync(SocketUserMessage message, string emoteName)
    {
        var emote = _client.Guilds
            .SelectMany(x => x.Emotes)
            .FirstOrDefault(x => x.Name.IndexOf(
                emoteName, StringComparison.InvariantCultureIgnoreCase) != -1);

        if (emote == null)
        {
            return;
        }

        await message.AddReactionAsync(emote);
    }
}