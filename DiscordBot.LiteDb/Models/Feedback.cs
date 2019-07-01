using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.LiteDb.Models
{
    public class Feedback
    {
        public ObjectId _id { get; set; }

        public string FullUserName { get; set; }

        public string FeedBack { get; set; }
    }
}
