using Discord.WebSocket;
using DiscordBot.Bot.Models;
using DiscordBot.LiteDb;
using DiscordBot.LiteDb.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Bot
{
    public class UserStateHandler
    {
        private Dictionary<ObjectId, UserState> _userStateStore;
        private LiteDbUnitOfWork _unitOfWork;

        public UserStateHandler(LiteDbUnitOfWork unitOfWork)
        {
            _userStateStore = new Dictionary<ObjectId, UserState>();
            _unitOfWork = unitOfWork;
        }

        public bool UserInDialog(UserState user)
        {
            if (!_userStateStore.ContainsKey(user.Key))
            {
                return false;
            }
            return _userStateStore[user.Key].DialogId != 0;
        }


        public void AdvanceToNewDialog(UserState user, string storyName, int DialogId)
        {
            if (_userStateStore.ContainsKey(user.Key))
            {
                _userStateStore[user.Key].StoryName = storyName;
                _userStateStore[user.Key].DialogId = DialogId;
            }
        }

        public UserState AddState(User user)
        {
            if (!_userStateStore.ContainsKey(user._id))
            {
                if (!_userStateStore.TryAdd(user._id, new UserState { User = user, Key = user._id }))
                {
                    throw new ApplicationException("Cannot create a userState");
                }
            }

            _userStateStore[user._id].User = user;

            return _userStateStore[user._id];
        }

        public UserState GetUserOrAdd(SocketMessage message)
        {
            var foundUser = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Name == message.Author.Username);

            if (foundUser != null)
            {
                foundUser.UserComments.Add(message.Content);
                _unitOfWork.Users.Update(foundUser);
            }
            else
            {
                foundUser = new User { Name = message.Author.Username };
                foundUser.UserComments.Add(message.Content);
                _unitOfWork.Users.Add(foundUser);
            }
            return AddState(foundUser);
        }

    }
}