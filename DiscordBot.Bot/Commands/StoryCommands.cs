using Discord;
using Discord.WebSocket;
using DiscordBot.Bot.Extensions;
using DiscordBot.Bot.Models;
using DiscordBot.LiteDb;
using DiscordBot.LiteDb.Models;
using DiscordBot.Stories;
using DiscordBot.Stories.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Bot.Commands
{
    public class StoryCommands
    {
        private UserStateHandler _stateHandler;
        private StoryStorageHandler _storageHandler;

        public StoryCommands(
            UserStateHandler stateHandler,
            StoryStorageHandler storageHandler)
        {
            _stateHandler = stateHandler;
            _storageHandler = storageHandler;
        }

        public async Task<CommandResult> StartStory(SocketMessage message, string[] parameters)
        {
            var dmChannel = await message.Author.GetOrCreateDMChannelAsync();

            var user = _stateHandler.GetUserOrAdd(message);

            var story = await _storageHandler.GetCachedStoryOrAdd("Campfire Story Demo");
            _stateHandler.AdvanceToNewDialog(user, story.ReadAbleName(), story.FirstStepId());

            await _displayStep(dmChannel, story.GetDialogStep(story.FirstStepId()));

            return new CommandResult { Message = "", Success = false };
        }

        public async Task ContinueStory(UserState userState, SocketMessage message)
        {
            var dmChannel = await message.Author.GetOrCreateDMChannelAsync();

            if (_stateHandler.UserInDialog(userState))
            {
                var story = await _storageHandler.GetCachedStoryOrAdd(userState.StoryName);

                var currentStep = story.GetDialogStep(userState.DialogId);

                var newStepId = _parseSelectedChoice(message, currentStep);

                _stateHandler.AdvanceToNewDialog(userState, story.ReadAbleName(), newStepId);

                await _displayStep(dmChannel, story.GetDialogStep(newStepId));
            }
        }

        private int _parseSelectedChoice(SocketMessage message, DialogStep currentStep)
        {
            var highest = 0;
            var highestKey = "";
            foreach (var choice in currentStep.PossibleChoices.Keys)
            {
                var current = choice.ToLower().WordMatches(message.Content.ToLower());
                if (current > highest && current > 0)
                {
                    highest = current;
                    highestKey = choice;
                }
            }
            return currentStep.PossibleChoices[highestKey];
        }

        private static async Task _displayStep(IDMChannel dmChannel, DialogStep currentStep)
        {
            if (!string.IsNullOrEmpty(currentStep.ImageUrl))
            {
                var emb = new EmbedBuilder() { ImageUrl = currentStep.ImageUrl };
                await dmChannel.SendMessageAsync("", false, emb.Build());
            }
            var nextDialog = new StoryFormatter()
                .AddText(currentStep);

            if(currentStep.PossibleChoices.Count > 0)
            {
                nextDialog.AddOptions(currentStep.PossibleChoices.Keys.ToList());
            }

            await dmChannel.SendMessageAsync(nextDialog.BuildDialog(), false);
        }
    }
}
