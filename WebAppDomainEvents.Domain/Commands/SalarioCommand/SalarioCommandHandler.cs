using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class SalarioCommandHandler : CommandHandler,
        INotificationHandler<AddSalarioCommand>, 
        INotificationHandler<EditSalarioCommand>,
        INotificationHandler<DeleteSalarioCommand>
    {
        private readonly ISalarioRepository _salarioRepository;

        public SalarioCommandHandler(IMediator mediator,
            ISalarioRepository salarioRepository) : base(mediator)
        {
            _salarioRepository = salarioRepository;
        }

        public Task Handle(AddSalarioCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                ReturnValidationErrors(command);
                return Task.CompletedTask;
            }

            _salarioRepository.AdicionarSalarioAsync(new Salario(command.Pagamento, command.Adiantamento));
            return Task.CompletedTask;
        }

        public Task Handle(EditSalarioCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                ReturnValidationErrors(command);
                return Task.CompletedTask;
            }

            _salarioRepository.EditarSalarioAsync(new Salario(command.Pagamento, command.Adiantamento));
            return Task.CompletedTask;
        }

        public Task Handle(DeleteSalarioCommand notification, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
