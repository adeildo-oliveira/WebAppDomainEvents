using FluentAssertions;
using MediatR;
using Moq;
using Moq.AutoMock;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tests.Shared.Builders.Commands;
using WebAppDomainEvents.Domain.Commands.DespesaMensalCommand;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Domain.Notifications;
using Xunit;

namespace Tests.Unit.Commands
{
    public class DespesaMensalCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly DespesaMensalCommandHandler _commandHandler;

        public DespesaMensalCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _commandHandler = _mocker.CreateInstance<DespesaMensalCommandHandler>();
        }

        [Fact]
        public async Task AddDespesaMensalCommandValidar()
        {
            var commandBuilder = new AddDespesaMensalCommandBuilder().Instanciar();
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
            commandBuilder.ValidationResult.Errors.Should().HaveCount(4);
            commandBuilder.IsValid().Should().BeFalse();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Exactly(4));
            _mocker.Verify<ISalarioRepository>(x => x.AdicionarSalarioAsync(It.IsAny<Salario>()), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.EditarSalarioAsync(It.IsAny<Salario>()), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.ObterSalarioPorIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task AddDespesaMensalCommand()
        {
            var salarioMock = new Salario(decimal.One, decimal.One);
            var idSalario = salarioMock.Id;

            var commandBuilder = new AddDespesaMensalCommandBuilder()
                .ComDescricao("Cartão")
                .ComIdSalario(idSalario)
                .ComData(DateTime.Now)
                .ComValor(decimal.One)
                .Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<ISalarioRepository>().Setup(x => x.EditarSalarioAsync(It.IsAny<Salario>()))
                .Returns(Task.CompletedTask)
                .Callback<Salario>((salario) =>
                {
                    salario.Id.Should().NotBeEmpty();
                    salario.Id.Should().Be(salarioMock.Id);
                    salario.Pagamento.Should().Be(salarioMock.Pagamento);
                    salario.Adiantamento.Should().Be(salarioMock.Adiantamento);
                    salario.Status.Should().BeTrue();
                    salario.DespesasMensais.Should().HaveCount(1);
                    var despesaMensal = salario.DespesasMensais.FirstOrDefault();
                    despesaMensal.Id.Should().NotBeEmpty();
                    despesaMensal.Descricao.Should().Be(commandBuilder.Descricao);
                    despesaMensal.Data.Should().Be(commandBuilder.Data);
                    despesaMensal.Valor.Should().Be(commandBuilder.Valor);
                    despesaMensal.Status.Should().BeTrue();
                });
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.ObterSalarioPorIdAsync(idSalario)).ReturnsAsync(salarioMock);
            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default)).Returns(Task.CompletedTask);

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.EditarSalarioAsync(salarioMock), Times.Once);
            _mocker.Verify<ISalarioRepository>(x => x.ObterSalarioPorIdAsync(idSalario), Times.Once);
        }

        [Fact]
        public async Task EditDespesaMensalCommandValidar()
        {
            var commandBuilder = new EditDespesaMensalCommandBuilder().Instanciar();
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
            commandBuilder.IsValid().Should().BeFalse();
            commandBuilder.ValidationResult.Errors.Should().HaveCount(5);
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Exactly(5));
            _mocker.Verify<ISalarioRepository>(x => x.AdicionarSalarioAsync(It.IsAny<Salario>()), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.EditarSalarioAsync(It.IsAny<Salario>()), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.ObterSalarioPorIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task EditDespesaMensalCommand()
        {
            var despesaMensalBuilder = new DespesaMensal("Celular", 32.99M, new DateTime(2019, 10, 5));
            var salarioBuilder = new Salario(1200.55M, 1500.89M).AdicionarDespesaMensal(despesaMensalBuilder);
            var commandBuilder = new EditDespesaMensalCommandBuilder()
                .ComId(despesaMensalBuilder.Id)
                .ComDescricao("Cartão")
                .ComIdSalario(new Guid("32cd6820-0da5-4c5f-94d1-e73b01f05de2"))
                .ComData(DateTime.Now)
                .ComValor(decimal.One)
                .Instanciar();
            commandBuilder.IsValid();
            
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.ObterSalarioPorIdAsync(commandBuilder.IdSalario)).ReturnsAsync(salarioBuilder);
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.EditarSalarioAsync(It.IsAny<Salario>()))
                .Returns(Task.CompletedTask)
                .Callback<Salario>((salario) =>
                {
                    salario.Id.Should().NotBeEmpty();
                    salario.Id.Should().Be(salarioBuilder.Id);
                    salario.Pagamento.Should().Be(salarioBuilder.Pagamento);
                    salario.Adiantamento.Should().Be(salarioBuilder.Adiantamento);
                    salario.Status.Should().BeTrue();
                    salario.DespesasMensais.Should().HaveCount(1);
                    var despesaMensal = salario.DespesasMensais.FirstOrDefault();
                    despesaMensal.Id.Should().NotBeEmpty();
                    despesaMensal.Descricao.Should().Be(commandBuilder.Descricao);
                    despesaMensal.Data.Should().Be(commandBuilder.Data);
                    despesaMensal.Valor.Should().Be(commandBuilder.Valor);
                    despesaMensal.Status.Should().BeTrue();
                });

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            commandBuilder.ValidationResult.Errors.Should().HaveCount(0);
            commandBuilder.IsValid().Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.AdicionarSalarioAsync(salarioBuilder), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.EditarSalarioAsync(salarioBuilder), Times.Once);
            _mocker.Verify<ISalarioRepository>(x => x.ObterSalarioPorIdAsync(commandBuilder.IdSalario), Times.Once);
        }

        [Fact]
        public async Task DeleteDespesaMensalCommandValidar()
        {
            var commandBuilder = new DeleteDespesaMensalCommandBuilder().ComStatus(true).Instanciar();
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
            commandBuilder.IsValid().Should().BeFalse();
            commandBuilder.ValidationResult.Errors.Should().HaveCount(3);
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Exactly(3));
            _mocker.Verify<ISalarioRepository>(x => x.AdicionarSalarioAsync(It.IsAny<Salario>()), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.EditarSalarioAsync(It.IsAny<Salario>()), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.ObterSalarioPorIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task DeleteDespesaMensalCommand()
        {
            var despesaMensalBuilder = new DespesaMensal("Celular", 32.99M, new DateTime(2019, 10, 5));
            var salarioBuilder = new Salario(1200.55M, 1500.89M)
                .AdicionarDespesaMensal(despesaMensalBuilder);

            var commandBuilder = new DeleteDespesaMensalCommandBuilder()
                .ComId(despesaMensalBuilder.Id)
                .ComIdSalario(new Guid("32cd6820-0da5-4c5f-94d1-e73b01f05de2"))
                .ComStatus(false)
                .Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<ISalarioRepository>().Setup(x => x.ObterSalarioPorIdAsync(commandBuilder.IdSalario)).ReturnsAsync(salarioBuilder);
            _mocker.GetMock<ISalarioRepository>().Setup(x => x.EditarSalarioAsync(It.IsAny<Salario>()))
                .Returns(Task.CompletedTask)
                .Callback<Salario>((salario) =>
                {
                    salario.Id.Should().NotBeEmpty();
                    salario.Id.Should().Be(salarioBuilder.Id);
                    salario.Pagamento.Should().Be(1200.55M);
                    salario.Adiantamento.Should().Be(1500.89M);
                    salario.Status.Should().BeTrue();
                    salario.DespesasMensais.Should().HaveCount(1);
                    var despesaMensal = salario.DespesasMensais.FirstOrDefault();
                    despesaMensal.Id.Should().NotBeEmpty();
                    despesaMensal.Descricao.Should().Be("Celular");
                    despesaMensal.Data.Should().Be(new DateTime(2019, 10, 5));
                    despesaMensal.Valor.Should().Be(32.99M);
                    despesaMensal.Status.Should().BeFalse();
                });

            var resultado = await _commandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            commandBuilder.ValidationResult.Errors.Should().HaveCount(0);
            commandBuilder.IsValid().Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.AdicionarSalarioAsync(salarioBuilder), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.EditarSalarioAsync(salarioBuilder), Times.Once);
            _mocker.Verify<ISalarioRepository>(x => x.ObterSalarioPorIdAsync(commandBuilder.IdSalario), Times.Once);
        }
    }
}
