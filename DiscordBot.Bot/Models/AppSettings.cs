using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Bot.Models
{
    public class AppSettings
    {
        public ulong AdminId { get; set; }

        public string DiscordToken { get; set; }

        public string StoriesLocation { get; set; }
    }
}
