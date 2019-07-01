using DiscordBot.LiteDb.Interfaces;
using LiteDB;
using System.Collections.Generic;

namespace DiscordBot.LiteDb
{
    public class LiteDbRepository<T> : IRepository<T> where T : class
    {
        private LiteCollection<T> _collection;

        public LiteDbRepository(LiteDatabase database)
        {
            _collection = database.GetCollection<T>();
        }

        public void Add(T item)
        {
            _collection.Insert(item);
        }

        public void Delete(T item)
        {
            var id = _getObjectId(item);

            _collection.Delete(Query.Where("_id", (val) => val.AsObjectId == id));
        }

        public IEnumerable<T> GetAll()
        {
            return _collection.FindAll();
        }

        public T GetById(ObjectId id)
        {
            return _collection.FindOne(Query.Where("_id", (val) => val.AsObjectId == id));
        }

        public void Update(T item)
        {
            _collection.Update(item);
        }

        private ObjectId _getObjectId(T item)
        {
            return item.GetType().GetProperty("_id").GetValue(item) as ObjectId;
        }
    }
}
