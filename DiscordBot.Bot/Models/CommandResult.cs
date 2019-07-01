using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Bot.Models
{
    public class CommandResult
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public EmbedBuilder Embed { get; set; }
    }
}
