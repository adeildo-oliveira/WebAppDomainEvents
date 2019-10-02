using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensalCommand
{
    public class DespesaMensalCommandHandler : CommandHandler,
        IRequestHandler<AddDespesaMensalCommand, bool>,
        IRequestHandler<EditDespesaMensalCommand, bool>,
        IRequestHandler<DeleteDespesaMensalCommand, bool>,
        IDisposable
    {
        private readonly ISalarioRepository _salarioRepository;

        public DespesaMensalCommandHandler(IMediator mediator, ISalarioRepository salarioRepository)
            : base(mediator) => _salarioRepository = salarioRepository;

        public async Task<bool> Handle(AddDespesaMensalCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                await ReturnValidationErrors(command);
                return await Task.FromResult(false);
            }

            var salario = await _salarioRepository.ObterSalarioPorIdAsync(command.Id);
            salario.AdicionarDespesaMensal(new DespesaMensal(command.Descricao, command.Valor, command.Data));

            await _salarioRepository.AdicionarSalarioAsync(salario);
            return await Task.FromResult(true);
        }

        public Task<bool> Handle(EditDespesaMensalCommand command, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<bool> Handle(DeleteDespesaMensalCommand command, CancellationToken cancellationToken) => throw new NotImplementedException();

        public void Dispose() => _salarioRepository.Dispose();
    }
}
