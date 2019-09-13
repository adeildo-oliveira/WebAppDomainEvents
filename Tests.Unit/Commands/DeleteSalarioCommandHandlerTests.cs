using FluentAssertions;
using MediatR;
using Moq;
using Moq.AutoMock;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tests.Shared.Builders.Commands;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Domain.Notifications;
using Xunit;

namespace Tests.Unit.Commands
{
    public class DeleteSalarioCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly SalarioCommandHandler _salarioCommandHandler;

        public DeleteSalarioCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _salarioCommandHandler = _mocker.CreateInstance<SalarioCommandHandler>();
        }

        [Fact]
        public async Task DeveValidarSalarioAntesDeExcluir()
        {
            var commandBuilder = new DeleteSalarioCommandBuilder().Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default))
                .Returns(Task.CompletedTask)
                .Callback<DomainNotification, CancellationToken>((notification, token) =>
                {
                    notification.Should().NotBeNull();
                    commandBuilder.ValidationResult.Errors
                    .FirstOrDefault(x => x.PropertyName == notification.Key && x.ErrorMessage == notification.Value)
                    .Should()
                    .NotBeNull();
                    token.IsCancellationRequested.Should().BeFalse();
                });

            var resultado = await _salarioCommandHandler.Handle(commandBuilder, default);

            resultado.Should().BeFalse();
            commandBuilder.ValidationResult.Errors.Should().HaveCount(1);
            commandBuilder.IsValid().Should().BeFalse();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Once);
            _mocker.Verify<ISalarioRepository>(x => x.ObterSalarioPorIdAsync(It.IsAny<Guid>()), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.RemoverSalarioAsync(It.IsAny<Salario>()), Times.Never);
        }

        [Fact]
        public async Task ExcluirSalarioCommand()
        {
            var commandBuilder = new DeleteSalarioCommandBuilder()
                .ComId(new Guid("32cd6820-0da5-4c5f-94d1-e73b01f05de2"))
                .ComStatus(false)
                .Instanciar();
            commandBuilder.IsValid();

            var salarioRetorno = new Salario(decimal.One, decimal.One);

            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default)).Returns(Task.CompletedTask);
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.ObterSalarioPorIdAsync(commandBuilder.Id)).ReturnsAsync(salarioRetorno);
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.RemoverSalarioAsync(It.IsAny<Salario>()))
                .Returns(Task.CompletedTask)
                .Callback<Salario>((salario) =>
                {
                    salario.Id.Should().NotBeEmpty();
                    salario.Id.Should().Be(commandBuilder.Id);
                    salario.Pagamento.Should().Be(salarioRetorno.Pagamento);
                    salario.Adiantamento.Should().Be(salarioRetorno.Adiantamento);
                    salario.Status.Should().BeFalse();
                });

            var resultado = await _salarioCommandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.ObterSalarioPorIdAsync(It.IsAny<Guid>()), Times.Once);
            _mocker.Verify<ISalarioRepository>(x => x.RemoverSalarioAsync(It.IsAny<Salario>()), Times.Once);
        }
    }
}
