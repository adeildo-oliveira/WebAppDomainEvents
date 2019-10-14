using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
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

        public EditSalarioCommandHandlerTests(DatabaseFixture fixture) : base(fixture)
        {
            _mediator = _fixture.Server.Services.GetService<IMediator>();
            _salarioRepository = _fixture.Server.Services.GetService<ISalarioRepository>();
            _notifications = (DomainNotificationHandler)_fixture.Server.Services.GetService<INotificationHandler<DomainNotification>>();
        }

        [Fact]
        public async Task DeveValidarSalarioAntesDeEditarESalvar()
        {
            var command = new EditSalarioCommand();

            var resultado = await _mediator.Send(command);

            resultado.Should().BeFalse();
            _notifications.HasNotifications().Should().BeTrue();
            _notifications.GetNotifications().Should().HaveCount(3);

            var resultadoBusca = await _salarioRepository.GetAllAsync();
            resultadoBusca.FirstOrDefault().Should().BeNull();
        }

        [Fact]
        public async Task DeveEditarUmSalario()
        {
            var salario = new Salario(12345.88M, 9875.00M);
            await _fixture.CriarAsync(salario);

            var command = new EditSalarioCommand
            {
                Id = salario.Id,
                Pagamento = 3178.62M,
                Adiantamento = 2819.75M
            };

            var resultado = await _mediator.Send(command);
            var resultadoBusca = await _salarioRepository.GetByIdAsync(command.Id);
            resultado.Should().BeTrue();
            resultadoBusca.Should().NotBeNull();
            resultadoBusca.Pagamento.Should().Be(command.Pagamento);
            resultadoBusca.Adiantamento.Should().Be(command.Adiantamento);
            resultadoBusca.Status.Should().BeTrue();
        }
    }
}
