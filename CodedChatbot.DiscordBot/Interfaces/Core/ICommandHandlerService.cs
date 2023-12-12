using System.Threading.Tasks;

namespace CodedChatbot.DiscordBot.Interfaces.Core;

public interface ICommandHandlerService
{
    Task InstallCommandsAsync();
}