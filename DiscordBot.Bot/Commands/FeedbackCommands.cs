using Discord.WebSocket;
using DiscordBot.Bot.Models;
using DiscordBot.LiteDb;
using DiscordBot.LiteDb.Models;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Bot.Commands
{
    public class FeedbackCommands
    {
        private LiteDbUnitOfWork _unitOfWork;
        private AppSettings _appSettings;

        public FeedbackCommands(LiteDbUnitOfWork unitOfWork, IOptions<AppSettings> options)
        {
            _unitOfWork = unitOfWork;
            _appSettings = options.Value;
        }

        public async Task<CommandResult> GetFeedBack(SocketMessage message, string[] input)
        {
            if (input.Length == 1)
            {
                return new CommandResult { Success = true, Message = "Please enter feedback in the following format \n .feedback {text-here}" };
            }

            _unitOfWork.Feedback.Add(new Feedback
            {
                FeedBack = string.Join(" ", input.Skip(1)),
                FullUserName = $"{message.Author.Username}#{message.Author.DiscriminatorValue}"
            });

            return new CommandResult { Message = "Thank you for submitting feedback of our Bot.", Success = true };
        }

        public async Task<CommandResult> GiveDukeFeedback(SocketMessage message, string[] input)
        {
            if (message.Author.Id == _appSettings.AdminId)
            {
                var stringBuilder = new StringBuilder();

                var allFeedback = _unitOfWork.Feedback.GetAll();

                stringBuilder.AppendLine($"Here are the results{Environment.NewLine}");

                foreach (var feedback in allFeedback)
                {
                    stringBuilder.AppendLine($"{feedback.FullUserName}: {feedback.FeedBack} {Environment.NewLine}");
                }

                return new CommandResult { Success = true, Message = stringBuilder.ToString() };
            }
            return new CommandResult { Success = false };
        }
    }
}
