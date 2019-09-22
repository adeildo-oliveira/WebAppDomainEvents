using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
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
    public class DeleteSalarioCommandHandlerTests : IntegrationTestFixture
    {
        private readonly IMediator _mediator;
        private readonly ISalarioRepository _salarioRepository;
        private readonly DomainNotificationHandler _notifications;

        public DeleteSalarioCommandHandlerTests()
        {
            _mediator = Service.GetService<IMediator>();
            _salarioRepository = Service.GetService<ISalarioRepository>();
            _notifications = (DomainNotificationHandler)Service.GetService<INotificationHandler<DomainNotification>>();
            Thread.Sleep(1000);
        }

        [Fact]
        public async Task DeveValidarSalarioAntesDeExcluir()
        {
            var command = new DeleteSalarioCommand { Status = true };

            var resultado = await _mediator.Send(command);

            resultado.Should().BeFalse();
            _notifications.HasNotifications().Should().BeTrue();
            _notifications.GetNotifications().Should().HaveCount(2);

            var resultadoBusca = await _salarioRepository.ObterSalarioAsync();
            resultadoBusca.FirstOrDefault().Should().BeNull();
        }

        [Fact]
        public async Task DeveValidarSeBuscaRetornouDadosParaoIdSalario()
        {
            var command = new DeleteSalarioCommand
            {
                Id = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Status = false
            };

            var resultado = await _mediator.Send(command);

            resultado.Should().BeTrue();
            _notifications.HasNotifications().Should().BeFalse();
            _notifications.GetNotifications().Should().HaveCount(0);

            var resultadoBusca = await _salarioRepository.ObterSalarioPorIdAsync(command.Id);
            resultadoBusca.Should().BeNull();
        }

        [Fact]
        public async Task DeveExcluirSalarioPeloIdSalario()
        {
            var salario = new Salario(12345.42M, 54321.24M)
                .AtualizarStatus(true);
            
            var command = new DeleteSalarioCommand
            {
                Id = salario.Id,
                Status = false
            };

            await Criar(salario);

            var resultado = await _mediator.Send(command);

            resultado.Should().BeTrue();
            _notifications.HasNotifications().Should().BeFalse();
            _notifications.GetNotifications().Should().HaveCount(0);

            var resultadoBusca = await _salarioRepository.ObterSalarioPorIdAsync(salario.Id);
            resultadoBusca.Should().NotBeNull();
            resultadoBusca.Id.Should().Be(salario.Id);
            resultadoBusca.Pagamento.Should().Be(salario.Pagamento);
            resultadoBusca.Adiantamento.Should().Be(salario.Adiantamento);
            resultadoBusca.Status.Should().BeFalse();
        }
    }
}
