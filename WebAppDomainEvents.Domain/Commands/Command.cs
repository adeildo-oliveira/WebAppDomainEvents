using FluentValidation.Results;
using MediatR;

namespace WebAppDomainEvents.Domain.Commands
{
    public abstract class Command : INotification
    {
        public abstract bool IsValid();
        public virtual ValidationResult ValidationResult { get; set; }
    }
}
