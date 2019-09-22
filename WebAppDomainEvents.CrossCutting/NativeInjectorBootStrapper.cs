using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;
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
            services.AddMediatR(AppDomain.CurrentDomain.Load("WebApi.DomainEvents"));
            DomainServices(services);
            InfraServices(services);
        }

        private static void InfraServices(IServiceCollection services)
        {
            services.AddScoped<DomainEventsContext>();
            services.AddScoped<ISalarioRepository, SalarioRepository>();
        }

        private static void DomainServices(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<AddSalarioCommand, bool>, AddSalarioCommandHandler>();
            services.AddScoped<IRequestHandler<EditSalarioCommand, bool>, EditSalarioCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteSalarioCommand, bool>, DeleteSalarioCommandHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
        }
    }
}
