using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAppDomainEvents.Domain.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private readonly ICollection<DomainNotification> _notifications;

        public DomainNotificationHandler() => _notifications = _notifications ?? new List<DomainNotification>();

        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            if (!_notifications.Any(x => x.Value == notification.Value))
                _notifications.Add(notification);

            return Task.CompletedTask;
        }

        public virtual IEnumerable<DomainNotification> GetNotifications() => _notifications;

        public virtual bool HasNotifications() => GetNotifications().Any();
    }
}
