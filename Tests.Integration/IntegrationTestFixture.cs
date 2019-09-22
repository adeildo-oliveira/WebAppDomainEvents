using Microsoft.Extensions.DependencyInjection;
using System;
using WebAppDomainEvents.CrossCutting;

namespace Tests.Integration
{
    public class IntegrationTestFixture : DatabaseFixture, IDisposable
    {
        public ServiceProvider Service;

        public IntegrationTestFixture()
        {
            ClearDataBase();

            var services = new ServiceCollection();
            NativeInjectorBootStrapper.RegisterServices(services);

            Service = services.BuildServiceProvider();
        }

        public new void Dispose()
        {
            Context.Dispose();
            Service.Dispose();
        }
    }
}
