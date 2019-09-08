using WebAppDomainEvents.Domain.Notifications;

namespace Tests.Shared.Builders
{
    public class DomainNotificationBuilder : InMemoryBuilder<DomainNotification>
    {
        public string _key;
        public string _value;

        public DomainNotificationBuilder Comkey(string key)
        {
            _key = key;
            return this;
        }

        public DomainNotificationBuilder ComValue(string value)
        {
            _value = value;
            return this;
        }

        public override DomainNotification Instanciar() => new DomainNotification(_key, _value);
    }
}
