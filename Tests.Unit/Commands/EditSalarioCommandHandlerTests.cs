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
    public class EditSalarioCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly SalarioCommandHandler _salarioCommandHandler;

        public EditSalarioCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _salarioCommandHandler = _mocker.CreateInstance<SalarioCommandHandler>();
        }

        [Fact]
        public async Task DeveValidarEdicaoDeSalarioAntesDeIncluir()
        {
            var commandBuilder = new EditSalarioCommandBuilder().Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default))
                .Returns(Task.CompletedTask)
                .Callback<DomainNotification, CancellationToken>((notification, token) =>
                {
                    notification.Should().NotBeNull();
                    commandBuilder.ValidationResult.Errors
                    .FirstOrDefault(x => x.PropertyName == notification.Key && x.ErrorMessage == notification.Value)
                    .Should().NotBeNull();
                    token.IsCancellationRequested.Should().BeFalse();
                });

            var resultado = await _salarioCommandHandler.Handle(commandBuilder, default);

            resultado.Should().BeFalse();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Exactly(3));
            _mocker.Verify<ISalarioRepository>(x => x.EditarSalarioAsync(It.IsAny<Salario>()), Times.Never);
        }

        [Fact]
        public async Task EditarSalarioCommand()
        {
            var commandBuilder = new EditSalarioCommandBuilder()
                .ComId(new Guid("32cd6820-0da5-4c5f-94d1-e73b01f05de2"))
                .ComPagamento(decimal.One)
                .ComAdiantamento(decimal.One)
                .Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default)).Returns(Task.CompletedTask);
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.EditarSalarioAsync(It.IsAny<Salario>()))
                .Returns(Task.CompletedTask)
                .Callback<Salario>((salario) =>
                {
                    salario.Id.Should().NotBeEmpty();
                    salario.Id.Should().Be(commandBuilder.Id);
                    salario.Pagamento.Should().Be(commandBuilder.Pagamento);
                    salario.Adiantamento.Should().Be(commandBuilder.Adiantamento);
                    salario.Status.Should().BeTrue();
                });

            var resultado = await _salarioCommandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.EditarSalarioAsync(It.IsAny<Salario>()), Times.Once);
        }
    }
}
