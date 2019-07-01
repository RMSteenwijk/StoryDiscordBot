using DiscordBot.Stories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBot.Stories.Models
{
    public class Story : IStory
    {

        public string Name { get; set; }

        public int FirstStep { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public List<DialogStep> Steps { get; set; } = new List<DialogStep>();

        public string FileName() => $"{Name}.json";
        public int FirstStepId() => FirstStep;
        public string ReadAbleName() => Name;
        public DialogStep GetDialogStep(int id) => Steps.FirstOrDefault(s => s.Id == id);
    }
}
