using MediatR;
using System;
using System.Linq;
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
        private readonly IDespesaMensalRepository _despesaMensalRepository;

        public DespesaMensalCommandHandler(IMediator mediator
            , ISalarioRepository salarioRepository
            , IDespesaMensalRepository despesaMensalRepository) : base(mediator)
        {
            _salarioRepository = salarioRepository;
            _despesaMensalRepository = despesaMensalRepository;
        }

        public async Task<bool> Handle(AddDespesaMensalCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                await ReturnValidationErrors(command);
                return false;
            }

            var salario = await _salarioRepository.GetByIdAsync(command.IdSalario, cancellationToken);

            if(salario != null)
            {
                var despesaMensal = new DespesaMensal(command.Descricao, command.Valor, command.Data).AdicionarSalario(salario);
                await _despesaMensalRepository.AddAsync(despesaMensal, cancellationToken);
            }

            return true;
        }

        public async Task<bool> Handle(EditDespesaMensalCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                await ReturnValidationErrors(command);
                return await Task.FromResult(false);
            }

            var salario = await _salarioRepository.GetByIdAsync(command.IdSalario, cancellationToken);
            var despesaMensal = salario?.DespesasMensais.FirstOrDefault(x => x.Id == command.Id);
            
            if(despesaMensal != null)
            {
                despesaMensal
                    .AtualizarDespesaMensal(command.Descricao, command.Valor, command.Data)
                    .AdicionarSalario(salario);
                await _despesaMensalRepository.UpdateAsync(despesaMensal, cancellationToken);
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> Handle(DeleteDespesaMensalCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                await ReturnValidationErrors(command);
                return await Task.FromResult(false);
            }

            var salario = await _salarioRepository.GetByIdAsync(command.IdSalario, cancellationToken);
            var despesaMensal = salario?.DespesasMensais.FirstOrDefault(x => x.Id == command.Id);

            if (despesaMensal != null)
            {
                despesaMensal
                    .AtualizarDespesaMensal(command.Status)
                    .AdicionarSalario(salario);
                await _despesaMensalRepository.DeleteAsync(despesaMensal, cancellationToken);
            }

            return await Task.FromResult(true);
        }

        public void Dispose() => _salarioRepository.Dispose();
    }
}
