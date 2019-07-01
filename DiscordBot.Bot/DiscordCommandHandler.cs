using Discord.WebSocket;
using DiscordBot.Bot.Commands;
using DiscordBot.Bot.Models;
using DiscordBot.Bot.Utils;
using DiscordBot.LiteDb;
using DiscordBot.LiteDb.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DiscordBot.Bot
{
    public class DiscordCommandHandler
    {
        private Dictionary<string, Func<SocketMessage, string[], Task<CommandResult>>> _commandToFunc = 
            new Dictionary<string, Func<SocketMessage, string[], Task<CommandResult>>>();

        private UserStateHandler _stateHandler;
        private ILogger<DiscordCommandHandler> _logger;

        private FeedbackCommands _feedbackComs;
        private StoryCommands _storyComs;

        public DiscordCommandHandler(
            LiteDbUnitOfWork unitOfWork, 
            UserStateHandler stateHandler, 
            IOptions<AppSettings> options,
            StoryCommands storyCommands,
            ILogger<DiscordCommandHandler> logger)
        {
            _stateHandler = stateHandler;
            _logger = logger;

            _feedbackComs = new FeedbackCommands(unitOfWork, options);
            _storyComs = storyCommands;

            AddCommand(".feedback", (msg, input) => _feedbackComs.GetFeedBack(msg, input));
            AddCommand(".getfeedback", (msg, input) => _feedbackComs.GiveDukeFeedback(msg, input));
            AddCommand(".start", (msg, input) => storyCommands.StartStory(msg, input));
        }

        public DiscordCommandHandler AddCommand(string command, Expression<Func<SocketMessage, string[], Task<CommandResult>>> expression)
        {
            _commandToFunc.Add(command, expression.Compile());
            return this;
        }

        public async Task ExecuteCommand(SocketMessage message)
        {
            using (var perfTimer = new PerformanceTimer<DiscordCommandHandler>(_logger, "Dialog Processing"))
            {
                var userState = _stateHandler.GetUserOrAdd(message);
                if (_stateHandler.UserInDialog(userState))
                {
                    await _storyComs.ContinueStory(userState, message);
                    return;
                }
            }
            

            if (_commandToFunc.TryGetValue(_tryFindCommand(message), out Func<SocketMessage, string[], Task<CommandResult>> commandFunc))
            {
                string[] parameters = message.Content.Split(" ").ToArray();

                var commandResult = await commandFunc(message, parameters);
                if (commandResult.Success)
                {
                    if (commandResult.Embed != null)
                    {
                        await message.Channel.SendMessageAsync(commandResult.Message, false, commandResult.Embed.Build());
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync(commandResult.Message);
                    }
                }
            }
        }

        private string _tryFindCommand(SocketMessage message)
        {
            var userInput = message.Content.ToLower();
            var detectedCommand = _commandToFunc.Keys
                .Select(k => k.ToLower())
                .FirstOrDefault(coms => coms == userInput);

            return string.IsNullOrEmpty(detectedCommand) ? "" : detectedCommand;
        }

        
    }
}
