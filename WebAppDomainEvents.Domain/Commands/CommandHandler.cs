using MediatR;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Notifications;

namespace WebAppDomainEvents.Domain.Commands
{
    public class CommandHandler
    {
        private readonly IMediator _mediator;

        public CommandHandler(IMediator mediator) => _mediator = mediator;

        protected async Task ReturnValidationErrors(Command command)
        {
            foreach (var error in command.ValidationResult.Errors)
            {
                await _mediator.Publish(new DomainNotification(error.PropertyName, error.ErrorMessage));
            }
        }
    }
}
