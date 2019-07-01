using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Stories.Models
{
    public class DialogStep
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public Dictionary<string, int> PossibleChoices { get; set; } = new Dictionary<string, int>();
    }
}
