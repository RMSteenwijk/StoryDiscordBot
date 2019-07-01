using DiscordBot.Stories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Stories.Interfaces
{
    public interface IStory
    {
        string FileName();

        string ReadAbleName();

        int FirstStepId();

        DialogStep GetDialogStep(int id);
    }
}
