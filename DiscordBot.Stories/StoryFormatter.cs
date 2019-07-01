using DiscordBot.Stories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Stories
{
    public class StoryFormatter
    {
        private StringBuilder _stringBuilder;

        public StoryFormatter()
        {
            _stringBuilder = new StringBuilder();
        }

        public StoryFormatter AddOptions(IList<string> stepNames)
        {
            _stringBuilder.AppendLine($"```");
            for (int i = 0; i < stepNames.Count; i++)
            {
                _stringBuilder.AppendLine($"> {stepNames[i]} {Environment.NewLine}");
            }
            _stringBuilder.Append("```");
            return this;
        }

        public StoryFormatter AddText(DialogStep step)
        {
            _stringBuilder.AppendLine(step.Text);
            return this;
        }

        public string BuildDialog()
        {
            return _stringBuilder.ToString();
        }
    }
}
