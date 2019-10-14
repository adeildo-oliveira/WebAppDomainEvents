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
    public class SalarioCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly SalarioCommandHandler _commandHandler;

        public SalarioCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _commandHandler = _mocker.CreateInstance<SalarioCommandHandler>();
        }

        [Fact]
        public async Task AddSalarioCommandValidar()
        {
            var commandBuilder = new AddSalarioCommandBuilder().Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default))
                .Returns(Task.CompletedTask)
                .Callback<DomainNotification, CancellationToken> ((notification, token) => 
                {
                    notification.Should().NotBeNull();
                    commandBuilder.ValidationResult.Errors
                    .FirstOrDefault(x => x.PropertyName == notification.Key && x.ErrorMessage == notification.Value)
                    .Should().NotBeNull();
                    token.IsCancellationRequested.Should().BeFalse();
                });

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeFalse();
            commandBuilder.ValidationResult.Errors.Should().HaveCount(2);
            commandBuilder.IsValid().Should().BeFalse();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Exactly(2));
            _mocker.Verify<ISalarioRepository>(x => x.AddAsync(It.IsAny<Salario>(), default), Times.Never);
        }

        [Fact]
        public async Task AddSalarioCommand()
        {
            var commandBuilder = new AddSalarioCommandBuilder()
                .ComPagamento(decimal.One)
                .ComAdiantamento(decimal.One)
                .Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<ISalarioRepository>().Setup(x => x.AddAsync(It.IsAny<Salario>(), default))
                .Returns(Task.CompletedTask)
                .Callback<Salario, CancellationToken>((objeto, token) =>
                {
                    objeto.Id.Should().NotBeEmpty();
                    objeto.Pagamento.Should().Be(commandBuilder.Pagamento);
                    objeto.Adiantamento.Should().Be(commandBuilder.Adiantamento);
                    objeto.Status.Should().BeTrue();
                });
            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default)).Returns(Task.CompletedTask);

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.AddAsync(It.IsAny<Salario>(), default), Times.Once);
        }

        [Fact]
        public async Task EditSalarioCommandValidar()
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

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeFalse();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Exactly(3));
            _mocker.Verify<ISalarioRepository>(x => x.UpdateAsync(It.IsAny<Salario>(), default), Times.Never);
        }

        [Fact]
        public async Task EditSalarioCommand()
        {
            var commandBuilder = new EditSalarioCommandBuilder()
                .ComId(new Guid("32cd6820-0da5-4c5f-94d1-e73b01f05de2"))
                .ComPagamento(decimal.One)
                .ComAdiantamento(decimal.One)
                .Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default)).Returns(Task.CompletedTask);
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.UpdateAsync(It.IsAny<Salario>(), default))
                .Returns(Task.CompletedTask)
                .Callback<Salario, CancellationToken>((objeto, token) =>
                {
                    objeto.Id.Should().NotBeEmpty();
                    objeto.Id.Should().Be(commandBuilder.Id);
                    objeto.Pagamento.Should().Be(commandBuilder.Pagamento);
                    objeto.Adiantamento.Should().Be(commandBuilder.Adiantamento);
                    objeto.Status.Should().BeTrue();
                });

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.UpdateAsync(It.IsAny<Salario>(), default), Times.Once);
        }

        [Fact]
        public async Task DeleteSalarioCommandValidar()
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

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeFalse();
            commandBuilder.ValidationResult.Errors.Should().HaveCount(1);
            commandBuilder.IsValid().Should().BeFalse();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Once);
            _mocker.Verify<ISalarioRepository>(x => x.GetByIdAsync(It.IsAny<Guid>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.DeleteAsync(It.IsAny<Salario>(), default), Times.Never);
        }

        [Fact]
        public async Task DeleteSalarioCommand()
        {
            var commandBuilder = new DeleteSalarioCommandBuilder()
                .ComId(new Guid("32cd6820-0da5-4c5f-94d1-e73b01f05de2"))
                .ComStatus(false)
                .Instanciar();
            commandBuilder.IsValid();

            var salarioRetorno = new Salario(decimal.One, decimal.One);

            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default)).Returns(Task.CompletedTask);
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.GetByIdAsync(commandBuilder.Id, default)).ReturnsAsync(salarioRetorno);
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.DeleteAsync(It.IsAny<Salario>(), default))
                .Returns(Task.CompletedTask)
                .Callback<Salario, CancellationToken>((objeto, token) =>
                {
                    objeto.Id.Should().NotBeEmpty();
                    objeto.Id.Should().Be(commandBuilder.Id);
                    objeto.Pagamento.Should().Be(salarioRetorno.Pagamento);
                    objeto.Adiantamento.Should().Be(salarioRetorno.Adiantamento);
                    objeto.Status.Should().BeFalse();
                });

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.GetByIdAsync(It.IsAny<Guid>(), default), Times.Once);
            _mocker.Verify<ISalarioRepository>(x => x.DeleteAsync(It.IsAny<Salario>(), default), Times.Once);
        }
    }
}
