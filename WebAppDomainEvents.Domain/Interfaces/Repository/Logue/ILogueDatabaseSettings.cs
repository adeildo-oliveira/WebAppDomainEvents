namespace WebAppDomainEvents.Domain.Interfaces.Repository.Logue
{
    public interface ILogueDatabaseSettings
    {
        string Server { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}
