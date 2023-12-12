using System.Threading.Tasks;
using CodedChatbot.DiscordBot.Helpers;
using Discord;
using Discord.Commands;

namespace CodedChatbot.DiscordBot.Commands;

public class SayCommand : ModuleBase<SocketCommandContext>
{
    [Command("say")]
    [Summary("Echoes a message...")]
    public async Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
    {
        if (!string.IsNullOrWhiteSpace(InputHelper.SanitiseInput(Format.StripMarkDown(echo))))
        {
            await ReplyAsync(echo);

            React
        }
    }
}