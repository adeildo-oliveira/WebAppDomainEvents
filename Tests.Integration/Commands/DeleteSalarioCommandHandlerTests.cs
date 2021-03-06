﻿using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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

        public DeleteSalarioCommandHandlerTests(DatabaseFixture fixture) : base(fixture)
        {
            _mediator = _fixture.Server.Services.GetService<IMediator>();
            _salarioRepository = _fixture.Server.Services.GetService<ISalarioRepository>();
            _notifications = (DomainNotificationHandler)_fixture.Server.Services.GetService<INotificationHandler<DomainNotification>>();
        }

        [Fact]
        public async Task DeveValidarSalarioAntesDeExcluir()
        {
            await _fixture.CriarAsync(new Salario(12345.42M, 54321.24M).AtualizarStatus(true));

            var command = new DeleteSalarioCommand { Status = true };
            var resultado = await _mediator.Send(command);

            resultado.Should().BeFalse();
            _notifications.HasNotifications().Should().BeTrue();
            _notifications.GetNotifications().Should().HaveCount(2);

            var resultadoBusca = await _salarioRepository.GetAllAsync();
            resultadoBusca.FirstOrDefault().Status.Should().BeTrue();
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

            var resultadoBusca = await _salarioRepository.GetByIdAsync(command.Id);
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

            await _fixture.CriarAsync(salario);

            var resultado = await _mediator.Send(command);

            resultado.Should().BeTrue();
            _notifications.HasNotifications().Should().BeFalse();
            _notifications.GetNotifications().Should().HaveCount(0);

            var resultadoBusca = await _fixture.BuscarAsync<Salario>(x => x.Id == salario.Id);
            resultadoBusca.Should().NotBeNull();
            resultadoBusca.Id.Should().Be(salario.Id);
            resultadoBusca.Pagamento.Should().Be(salario.Pagamento);
            resultadoBusca.Adiantamento.Should().Be(salario.Adiantamento);
            resultadoBusca.Status.Should().BeFalse();
        }
    }
}
