using WebAppDomainEvents.Domain.Interfaces.Repository.Logue;

namespace WebAppDomainEvents.Infra.Repository.Logue
{
    public class LogueDatabaseSettings : ILogueDatabaseSettings
    {
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
