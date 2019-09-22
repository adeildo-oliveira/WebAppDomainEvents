using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class AddSalarioCommandHandler : CommandHandler, IRequestHandler<AddSalarioCommand, bool>, IDisposable
    {
        private readonly ISalarioRepository _salarioRepository;

        public AddSalarioCommandHandler(IMediator mediator, ISalarioRepository salarioRepository) 
            : base(mediator) => _salarioRepository = salarioRepository;

        public async Task<bool> Handle(AddSalarioCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                await ReturnValidationErrors(command);
                return await Task.FromResult(false);
            }

            await _salarioRepository.AdicionarSalarioAsync(new Salario(command.Pagamento, command.Adiantamento));
            return await Task.FromResult(true);
        }

        public void Dispose() => _salarioRepository.Dispose();
    }
}
