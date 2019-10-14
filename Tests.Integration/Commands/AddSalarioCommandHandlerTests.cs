using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Notifications;
using Xunit;

namespace Tests.Integration.Commands
{
    public class AddSalarioCommandHandlerTests : IntegrationTestFixture
    {
        private readonly IMediator _mediator;
        private readonly ISalarioRepository _salarioRepository;
        private readonly DomainNotificationHandler _notifications;

        public AddSalarioCommandHandlerTests(DatabaseFixture fixture) : base(fixture)
        {
            _mediator = _fixture.Server.Services.GetService<IMediator>();
            _salarioRepository = _fixture.Server.Services.GetService<ISalarioRepository>();
            _notifications = (DomainNotificationHandler)_fixture.Server.Services.GetService<INotificationHandler<DomainNotification>>();
        }

        [Fact]
        public async Task DeveValidarSalarioAntesDeIncluir()
        {
            var command = new AddSalarioCommand { };

            var resultado = await _mediator.Send(command);

            resultado.Should().BeFalse();
            _notifications.HasNotifications().Should().BeTrue();
            _notifications.GetNotifications().Should().HaveCount(2);

            var resultadoBusca = await _salarioRepository.GetAllAsync();
            resultadoBusca.FirstOrDefault().Should().BeNull();
        }

        [Fact]
        public async Task DeveIncluirUmSalario()
        {
            var command = new AddSalarioCommand
            {
                Pagamento = 1200.24M,
                Adiantamento = 800.60M
            };

            var resultado = await _mediator.Send(command);

            resultado.Should().BeTrue();
            _notifications.HasNotifications().Should().BeFalse();
            _notifications.GetNotifications().Should().HaveCount(0);

            var resultadoBusca = (await _salarioRepository.GetAllAsync()).FirstOrDefault(x => x.Pagamento == command.Pagamento);
            resultadoBusca.Should().NotBeNull();
            resultadoBusca.Pagamento.Should().Be(command.Pagamento);
            resultadoBusca.Adiantamento.Should().Be(command.Adiantamento);
            resultadoBusca.Status.Should().BeTrue();
        }
    }
}
