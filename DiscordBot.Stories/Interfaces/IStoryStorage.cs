using DiscordBot.Stories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Stories.Interfaces
{
    public interface IStoryStorage
    {
        Task StoreStory(IStory story);
        Task<IStory> LoadStory(string name); 
        List<string> ListStories();

    }
}
