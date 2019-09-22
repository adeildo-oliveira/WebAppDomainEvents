using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class DeleteSalarioCommandHandler : CommandHandler, IRequestHandler<DeleteSalarioCommand, bool>, IDisposable
    {
        private readonly ISalarioRepository _salarioRepository;

        public DeleteSalarioCommandHandler(IMediator mediator, ISalarioRepository salarioRepository)
            : base(mediator) => _salarioRepository = salarioRepository;

        public async Task<bool> Handle(DeleteSalarioCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                await ReturnValidationErrors(command);
                return await Task.FromResult(false);
            }

            var resultado = await _salarioRepository.ObterSalarioPorIdAsync(command.Id);

            if(resultado != null)
                await _salarioRepository.RemoverSalarioAsync(
                    new Salario(resultado.Pagamento, resultado.Adiantamento)
                    .AtualizarId(command.Id)
                    .AtualizarStatus(command.Status));

            return await Task.FromResult(true);
        }

        public void Dispose() => _salarioRepository.Dispose();
    }
}
