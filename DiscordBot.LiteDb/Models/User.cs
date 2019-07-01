using LiteDB;
using System;
using System.Collections.Generic;

namespace DiscordBot.LiteDb.Models
{
    public class User
    {
        public ObjectId _id { get; set; }

        public ulong DiscordUserId { get; set; } 

        public string Name { get; set; }

        public List<string> UserComments { get; set; } = new List<string>();
    }
}
