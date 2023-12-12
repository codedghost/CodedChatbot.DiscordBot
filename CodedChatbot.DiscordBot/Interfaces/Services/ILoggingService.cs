using System.Threading.Tasks;
using Discord;

namespace CodedChatbot.DiscordBot.Interfaces.Services;

public interface ILoggingService
{
    Task Log(LogMessage message);
}