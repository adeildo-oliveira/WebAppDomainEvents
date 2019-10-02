using FluentAssertions;
using MediatR;
using Moq;
using Moq.AutoMock;
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
        }
    }
}
