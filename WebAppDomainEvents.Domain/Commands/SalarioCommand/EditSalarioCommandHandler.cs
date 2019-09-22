using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class EditSalarioCommandHandler : CommandHandler, IRequestHandler<EditSalarioCommand, bool>, IDisposable
    {
        private readonly ISalarioRepository _salarioRepository;

        public EditSalarioCommandHandler(IMediator mediator, ISalarioRepository salarioRepository)
            : base(mediator) => _salarioRepository = salarioRepository;

        public async Task<bool> Handle(EditSalarioCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                await ReturnValidationErrors(command);
                return await Task.FromResult(false);
            }

            await _salarioRepository.EditarSalarioAsync(
                new Salario(command.Pagamento, command.Adiantamento).AtualizarId(command.Id));

            return await Task.FromResult(true);
        }

        public void Dispose() => _salarioRepository.Dispose();
    }
}
