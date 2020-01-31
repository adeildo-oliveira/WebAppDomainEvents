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
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddControllers(x => 
            {
                x.ReturnHttpNotAcceptable = true;
                //x.RequireHttpsPermanent = true;
            });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("pt-BR");
            });

            services.DomainServices();
            services.InfraServices();
        }

        private static void InfraServices(this IServiceCollection services)
        {
            services.AddScoped<DomainEventsContext>();
            services.AddScoped<ISalarioRepository, SalarioRepository>();
            services.AddScoped<ISalarioRepositoryReadOnly, SalarioRepositoryReadOnly>();
            services.AddScoped<IDespesaMensalRepository, DespesaMensalRepository>();
            services.AddScoped<IDespesaMensalRepositoryReadOnly, DespesaMensalRepositoryReadOnly>();
        }

        private static void DomainServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.Load("WebAppDomainEvents.Domain"));
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
        }
    }
}
