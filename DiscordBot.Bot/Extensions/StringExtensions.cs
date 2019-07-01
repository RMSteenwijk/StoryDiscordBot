using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBot.Bot.Extensions
{
    public static class StringExtensions
    {
        public static int WordMatches(this string theString, string otherString)
        {
            return theString.Split().Intersect(otherString.Split()).Count();
        }
    }
}
