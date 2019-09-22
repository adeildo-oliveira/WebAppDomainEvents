using FluentAssertions;
using MediatR;
using Moq;
using Moq.AutoMock;
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
    public class AddSalarioCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly AddSalarioCommandHandler _salarioCommandHandler;

        public AddSalarioCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _salarioCommandHandler = _mocker.CreateInstance<AddSalarioCommandHandler>();
        }

        [Fact]
        public async Task DeveValidarSalarioAntesDeIncluir()
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

            var resultado = await _salarioCommandHandler.Handle(commandBuilder, default);

            resultado.Should().BeFalse();
            commandBuilder.ValidationResult.Errors.Should().HaveCount(2);
            commandBuilder.IsValid().Should().BeFalse();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Exactly(2));
            _mocker.Verify<ISalarioRepository>(x => x.AdicionarSalarioAsync(It.IsAny<Salario>()), Times.Never);
        }

        [Fact]
        public async Task AdicionarSalarioCommand()
        {
            var commandBuilder = new AddSalarioCommandBuilder()
                .ComPagamento(decimal.One)
                .ComAdiantamento(decimal.One)
                .Instanciar();
            commandBuilder.IsValid();

            _mocker.GetMock<ISalarioRepository>().Setup(x => x.AdicionarSalarioAsync(It.IsAny<Salario>()))
                .Returns(Task.CompletedTask)
                .Callback<Salario>((salario) => 
                {
                    salario.Id.Should().NotBeEmpty();
                    salario.Pagamento.Should().Be(commandBuilder.Pagamento);
                    salario.Adiantamento.Should().Be(commandBuilder.Adiantamento);
                    salario.Status.Should().BeTrue();
                });
            _mocker.GetMock<IMediator>().Setup(x => x.Publish(It.IsAny<DomainNotification>(), default)).Returns(Task.CompletedTask);

            var resultado = await _salarioCommandHandler.Handle(commandBuilder, default);

            resultado.Should().BeTrue();
            _mocker.Verify<IMediator>(x => x.Publish(It.IsAny<DomainNotification>(), default), Times.Never);
            _mocker.Verify<ISalarioRepository>(x => x.AdicionarSalarioAsync(It.IsAny<Salario>()), Times.Once);
        }
    }
}
