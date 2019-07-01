using DiscordBot.Stories.Models;
using System;
using System.Collections.Generic;

namespace DiscordBot.Stories
{
    public class StorySeeder
    {

        public static Story CreateStory()
        {
            return new Story
            {
                FirstStep = 1,
                Name = "Campfire Story Demo",
                Tags = new List<Tag> { new Tag { Name = "Campfire" }, new Tag { Name = "Story" } },
                Steps = new List<DialogStep>
                {
                    new DialogStep
                    {
                        Id = 1,
                        ImageUrl = "https://i.ytimg.com/vi/qsOUv9EzKsg/maxresdefault.jpg",
                        Text = "U arrive at a campfire in the middle of the night. What is your next move?",
                        PossibleChoices = new Dictionary<string, int>
                        {
                            { "Check for people", 2 },
                            { "Sleep", 3 },
                            { "Move on", 4 }
                        }
                    },
                    new DialogStep
                    {
                        Id = 2,
                        ImageUrl = "https://i.pinimg.com/originals/ec/44/1b/ec441b1d6d45c8ecbefa2c52115c239f.jpg",
                        Text = "U notice a tarp large enough to conceal a person.",
                        PossibleChoices = new Dictionary<string, int>
                        {
                            { "Pull Tarp", 5 },
                            { "Look at campfire again", 1 },
                        }
                    },
                    new DialogStep
                    {
                        Id = 3,
                        ImageUrl = "http://cache.desktopnexus.com/thumbseg/2179/2179929-bigthumbnail.jpg",
                        Text = "Exhausted you lay your head to rest on a nearby branch and close your eyes as you try to sleep. During the night you hear a small schuffle before a " +
                        "sudden burning pain to your throat make your realise somebody has slit your throat. Please try again.",
                        PossibleChoices = new Dictionary<string, int>
                        {
                            { "Try again", 1 },
                        }
                    },
                    new DialogStep
                    {
                        Id = 4,
                        ImageUrl = "https://i.ytimg.com/vi/qsOUv9EzKsg/maxresdefault.jpg",
                        Text = "Exhausted you keep moving since you don't trust the any lucky encouters such as a warm fire. ",
                        PossibleChoices = new Dictionary<string, int>
                        {
                            { "Go back", 1 },
                        }
                    },
                    new DialogStep
                    {
                        Id = 5,
                        ImageUrl = "http://pm1.narvii.com/6613/7fbe225f6a09edfd2a16a08c5924b07ce58abb7b_00.jpg",
                        Text = "End of Demo you found ur Waifu.",
                        PossibleChoices = new Dictionary<string, int>
                        {

                        }
                    },
                }
            };
        }
    }
}
