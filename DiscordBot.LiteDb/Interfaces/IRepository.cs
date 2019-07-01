using LiteDB;
using System;
using System.Collections.Generic;

namespace DiscordBot.LiteDb.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T item);

        void Update(T item);

        void Delete(T item);

        T GetById(ObjectId id);

        IEnumerable<T> GetAll();
    }
}