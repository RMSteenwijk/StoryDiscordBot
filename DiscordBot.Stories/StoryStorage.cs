using DiscordBot.Stories.Interfaces;
using DiscordBot.Stories.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Stories
{
    public class StoryStorage : IStoryStorage
    {
        private string _storyFolderPath;

        public StoryStorage(string storyFolderPath)
        {
            _storyFolderPath = storyFolderPath;
        }

        public async Task StoreStory(IStory story)
        {
            if (!Directory.Exists(_storyFolderPath))
                Directory.CreateDirectory(_storyFolderPath);

            await File.WriteAllTextAsync(Path.Combine(_storyFolderPath, story.FileName()), JsonConvert.SerializeObject(story));
        }

        public async Task<IStory> LoadStory(string name)
        {
            return JsonConvert.DeserializeObject<Story>(await File.ReadAllTextAsync(Path.Combine(_storyFolderPath, $"{name}.json")));
        }

        public List<string> ListStories()
        {
            return Directory.GetFiles(_storyFolderPath, "*.json", SearchOption.AllDirectories)
                .ToList()
                .Select(path => Path.GetFileNameWithoutExtension(path))
                .ToList();
        }
 
    }
}
