using MongoDB.Driver;
using WebAppDomainEvents.Domain.Interfaces.Repository.Logue;

namespace WebAppDomainEvents.Infra.Repository.Logue
{
    public class LogueMongodb<T> : ILogueMongodb<T> where T : class
    {
        private readonly IMongoCollection<T> _mongoCollection;

        public LogueMongodb(ILogueDatabaseSettings settings)
        {
            var client = new MongoClient(settings.Server);
            var database = client.GetDatabase(settings.DatabaseName);
            _mongoCollection = database.GetCollection<T>(settings.CollectionName);
        }

        public virtual IMongoCollection<T> GetMongoCollection() => _mongoCollection;
    }
}
