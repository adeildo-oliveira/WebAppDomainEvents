using FluentAssertions;
using System.Linq;
using Tests.Shared.Builders;
using WebAppDomainEvents.Domain.Notifications;
using Xunit;

namespace Tests.Unit
{
    public class DomainNotificationHandlerTests
    {
        private readonly DomainNotificationHandler _domainNotification;

        public DomainNotificationHandlerTests() => _domainNotification = new DomainNotificationHandler();

        [Fact]
        public void DeveAdicionarUmaNotification()
        {
            var notificationBuilder = new DomainNotificationBuilder()
                .Comkey("Id")
                .ComValue("Id inválido")
                .Instanciar();
            
            _domainNotification.Handle(notificationBuilder, default);

            _domainNotification.GetNotifications().Should().NotBeNull();
            _domainNotification.GetNotifications().Should().HaveCount(1);
            _domainNotification.GetNotifications().Should().BeEquivalentTo(notificationBuilder);
            _domainNotification.HasNotifications().Should().BeTrue();
        }

        [Fact]
        public void DeveAdicionarMaisDeUmaNotification()
        {
            var notificationBuilder = new DomainNotificationBuilder()
                .Comkey("Id")
                .ComValue("Id inválido")
                .Instanciar();

            var notificationBuilder2 = new DomainNotificationBuilder()
                .ComValue("Cliente Inválido")
                .Instanciar();

            _domainNotification.Handle(notificationBuilder, default);
            _domainNotification.Handle(notificationBuilder2, default);

            var resultado = _domainNotification.GetNotifications().ToList();

            _domainNotification.GetNotifications().Should().NotBeNull();
            _domainNotification.GetNotifications().Should().HaveCount(2);
            resultado[0].Should().BeEquivalentTo(notificationBuilder);
            resultado[1].Should().BeEquivalentTo(notificationBuilder2);
            _domainNotification.HasNotifications().Should().BeTrue();
        }
    }
}
