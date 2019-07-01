using DiscordBot.LiteDb.Models;
using LiteDB;

namespace DiscordBot.Bot.Models
{
    public class UserState
    {
        public ObjectId Key { get; set; } 

        public User User { get; set; }

        public string StoryName { get; set; }

        public int DialogId { get; set; }
    }
}