using MediatR;

namespace WebAppDomainEvents.Domain.Notifications
{
    public class DomainNotification : INotification
    {
        public DomainNotification(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; private set; }
        public string Value { get; private set; }
    }
}
