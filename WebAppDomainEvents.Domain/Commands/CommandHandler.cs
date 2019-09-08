using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Notifications;

namespace WebAppDomainEvents.Domain.Commands
{
    public class CommandHandler
    {
        private readonly IMediator _mediator;

        public CommandHandler(IMediator mediator) => _mediator = mediator;

        protected Task ReturnValidationErrors(Command command)
        {
            ReturnDomainNotificationErrors(command);
            return Task.CompletedTask;
        }

        private void ReturnDomainNotificationErrors(Command command)
        {
            foreach (var error in command.ValidationResult.Errors)
            {
                _mediator.Publish(new DomainNotification(error.PropertyName, error.ErrorMessage));
            }
        }
    }
}
