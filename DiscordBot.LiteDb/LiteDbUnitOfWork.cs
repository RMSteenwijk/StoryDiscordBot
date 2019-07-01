using DiscordBot.LiteDb.Interfaces;
using DiscordBot.LiteDb.Models;
using LiteDB;
using System;

namespace DiscordBot.LiteDb
{
    public class LiteDbUnitOfWork : IUnitOfWork, IDisposable
    {
        public IRepository<User> Users { get { return new LiteDbRepository<User>(_database); } }

        public IRepository<Feedback> Feedback { get { return new LiteDbRepository<Feedback>(_database); } }

        private LiteDatabase _database;

        public LiteDbUnitOfWork(string connectionString)
        {
            _database = new LiteDatabase(connectionString);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _database.Dispose();
                }
                _database = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LiteDbUnitOfWork()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
