using Microsoft.Extensions.DependencyInjection;
using WebAppDomainEvents.CrossCutting;
using Xunit;

namespace Tests.Integration
{
    [Collection(Name)]
    public class IntegrationTestFixture : IClassFixture<DatabaseFixture>
    {
        public const string Name = nameof(IntegrationTestFixture);
        public ServiceProvider Service;
        public readonly DatabaseFixture _fixture;

        public IntegrationTestFixture(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearDataBase();

            var services = new ServiceCollection();
            NativeInjectorBootStrapper.RegisterServices(services);
            
            Service = services.BuildServiceProvider();
        }
    }
}
