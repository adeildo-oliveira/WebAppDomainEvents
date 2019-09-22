using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Domain.Notifications;
using Xunit;

namespace Tests.Integration.Commands
{
    public class EditSalarioCommandHandlerTests : IntegrationTestFixture
    {
        private readonly IMediator _mediator;
        private readonly ISalarioRepository _salarioRepository;
        private readonly DomainNotificationHandler _notifications;

        public EditSalarioCommandHandlerTests()
        {
            _mediator = Service.GetService<IMediator>();
            _salarioRepository = Service.GetService<ISalarioRepository>();
            _notifications = (DomainNotificationHandler)Service.GetService<INotificationHandler<DomainNotification>>();
            Thread.Sleep(2000);
        }

        [Fact]
        public async Task DeveValidarSalarioAntesDeEditarESalvar()
        {
            var command = new EditSalarioCommand();

            var resultado = await _mediator.Send(command);

            resultado.Should().BeFalse();
            _notifications.HasNotifications().Should().BeTrue();
            _notifications.GetNotifications().Should().HaveCount(3);

            var resultadoBusca = await _salarioRepository.ObterSalarioAsync();
            resultadoBusca.FirstOrDefault().Should().BeNull();
        }

        [Fact]
        public async Task DeveEditarUmSalario()
        {
            var salario = new Salario(12345.88M, 9875.00M);
            await Criar(salario);

            var command = new EditSalarioCommand
            {
                Id = salario.Id,
                Pagamento = 3178.62M,
                Adiantamento = 2819.75M
            };

            var resultado = await _mediator.Send(command);
            var resultadoBusca = await _salarioRepository.ObterSalarioPorIdAsync(command.Id);
            resultado.Should().BeTrue();
            resultadoBusca.Should().NotBeNull();
            resultadoBusca.Pagamento.Should().Be(command.Pagamento);
            resultadoBusca.Adiantamento.Should().Be(command.Adiantamento);
            resultadoBusca.Status.Should().BeTrue();
        }
    }
}
