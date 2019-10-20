using MongoDB.Driver;

namespace WebAppDomainEvents.Domain.Interfaces.Repository.Logue
{
    public interface ILogueMongodb<T> where T : class
    {
        IMongoCollection<T> GetMongoCollection();
    }
}
