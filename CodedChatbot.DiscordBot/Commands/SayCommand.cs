using System.Threading.Tasks;
using Discord.Commands;

namespace CodedChatbot.DiscordBot.Commands;

public class SayCommand : ModuleBase<SocketCommandContext>
{
    [Command("say")]
    [Summary("Echoes a message...")]
    public async Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
    {
        await ReplyAsync(echo);
    }
}