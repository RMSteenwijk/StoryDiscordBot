using Discord;
using Discord.WebSocket;
using DiscordBot.Bot.Commands;
using DiscordBot.Bot.Models;
using DiscordBot.LiteDb;
using DiscordBot.Stories;
using DiscordBot.Stories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Bot
{
    public class MainBot
    {
        private DiscordSocketClient _client;
        private ILogger<MainBot> _logger;
        private ServiceProvider _serviceProvider;
        private AppSettings _appSettings;

        public static void Main(string[] args)
        => new MainBot().MainAsync().GetAwaiter().GetResult();


        public async Task MainAsync()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            _appSettings = configuration.GetSection("Configuration").Get<AppSettings>();

            _serviceProvider = new ServiceCollection()
                .AddScoped((service) => { return new LiteDbUnitOfWork(configuration.GetConnectionString("LiteDb")); })
                .AddScoped<StoryCommands>()
                .AddSingleton<DiscordCommandHandler>()
                .AddSingleton<UserStateHandler>()
                .AddSingleton<StoryStorageHandler>()
                .AddMemoryCache()
                .AddSingleton<IStoryStorage>((services) => { return new StoryStorage(_appSettings.StoriesLocation); })
                .Configure<AppSettings>(configuration.GetSection("Configuration"))
                .AddLogging((logBuilder) => { logBuilder.AddConsole(); })
                .BuildServiceProvider();

            _logger = _serviceProvider.GetService<ILogger<MainBot>>();

            await _serviceProvider.GetService<StoryStorageHandler>().StoreStory(StorySeeder.CreateStory());

            await _startDiscordBot();
        }

        private async Task _startDiscordBot()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, _appSettings.DiscordToken);
            await _client.StartAsync();

            _client.MessageReceived += MessageReceived;

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            _logger.LogInformation(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.Username == _client.CurrentUser.Username) //Don't process own messages?
            {
                return;
            }
            _logger.LogInformation($"{message.Author.Username}: {message.Content}");

            var commandHandler = _serviceProvider.GetService<DiscordCommandHandler>();
            try
            {
                await commandHandler.ExecuteCommand(message);
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
