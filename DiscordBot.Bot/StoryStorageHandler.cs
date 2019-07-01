using DiscordBot.Stories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Bot
{
    public class StoryStorageHandler
    {
        private IStoryStorage _storyStorage;
        private IMemoryCache _memoryCache;

        public StoryStorageHandler(IStoryStorage storyStorage, IMemoryCache memoryCache)
        {
            _storyStorage = storyStorage;
            _memoryCache = memoryCache;
        }

        public async Task<IStory> GetCachedStoryOrAdd(string name)
        {
            return await _memoryCache.GetOrCreateAsync(name, (entry) => _storyStorage.LoadStory(name));
        }


        public List<string> GetAllStories()
        {
            return _storyStorage.ListStories();
        }

        public async Task StoreStory(IStory story)
        {
            await _storyStorage.StoreStory(story);
        }

        public void ClearCachedStory(string name)
        {
            _memoryCache.Remove(name);
        }
    }
}
