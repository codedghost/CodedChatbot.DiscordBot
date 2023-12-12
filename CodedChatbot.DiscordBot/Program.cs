using System.Threading.Tasks;
using CodedChatbot.DiscordBot.Interfaces.Core;
using CodedChatbot.DiscordBot.Interfaces.Services;
using CoreCodedChatbot.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodedChatbot.DiscordBot;

public class Program
{
    static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

    static async Task MainAsync(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var secretService = await builder.Services
            .AddChatbotServices();

        builder.Services.AddChatbotNLog();

        // build services so that we can configure the client before rebuilding for main app
        var serviceProvider = builder.Services.BuildServiceProvider();

        await builder.Services
            .AddConfiguredDiscordClient(secretService, serviceProvider);

        builder.Services.AddDiscordBotServices();

        serviceProvider = builder.Services.BuildServiceProvider();

        var clientService = serviceProvider.GetService<ICommandHandlerService>();

        if (clientService != null)
        {
            await clientService.InstallCommandsAsync();
        }

        var host = builder.Build();

        await host.RunAsync();
    }
}
