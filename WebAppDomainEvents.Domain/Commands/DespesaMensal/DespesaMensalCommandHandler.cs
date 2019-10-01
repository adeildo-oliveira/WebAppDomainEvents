using MediatR;
using WebAppDomainEvents.Domain.Interfaces.Repository;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensal
{
    public class DespesaMensalCommandHandler : CommandHandler
    {
        private readonly ISalarioRepository _salarioRepository;

        public DespesaMensalCommandHandler(IMediator mediator, ISalarioRepository salarioRepository)
            : base(mediator) => _salarioRepository = salarioRepository;
    }
}
