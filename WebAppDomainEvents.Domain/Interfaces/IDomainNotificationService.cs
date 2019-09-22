using FluentValidation.Results;
using System.Collections.Generic;
using WebAppDomainEvents.Domain.Notifications;

namespace WebAppDomainEvents.Domain.Interfaces
{
    public interface IDomainNotificationService
    {
        void AddNotification(DomainNotification notification);
        void RemoveNotification(DomainNotification notification);
        IReadOnlyCollection<DomainNotification> GetNotifications();
        void AddValidationResult(ValidationResult validationResult);
        bool HasNotifications();
    }
}
