using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Notifications;
using WebAppDomainEvents.Infra.Context;
using WebAppDomainEvents.Infra.Repository;

namespace WebAppDomainEvents.CrossCutting
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("pt-BR");
            });

            DomainServices(services);
            InfraServices(services);
        }

        private static void InfraServices(IServiceCollection services)
        {
            services.AddScoped<DomainEventsContext>();
            services.AddScoped<ISalarioRepository, SalarioRepository>();
            services.AddScoped<ISalarioRepositoryReadOnly, SalarioRepositoryReadOnly>();
            services.AddScoped<IDespesaMensalRepository, DespesaMensalRepository>();
            services.AddScoped<IDespesaMensalRepositoryReadOnly, DespesaMensalRepositoryReadOnly>();
        }

        private static void DomainServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.Load("WebAppDomainEvents.Domain"));
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
        }
    }
}
