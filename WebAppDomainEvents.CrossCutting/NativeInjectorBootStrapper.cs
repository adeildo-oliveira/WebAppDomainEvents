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
            MapperServices(services);
            DomainServices(services);
            InfraServices(services);
        }

        private static void MapperServices(IServiceCollection services)
        {

        }

        private static void InfraServices(IServiceCollection services)
        {
            services.AddScoped<DomainEventsContext>();
            services.AddScoped<ISalarioRepository, SalarioRepository>();
        }

        private static void DomainServices(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<AddSalarioCommand, bool>, SalarioCommandHandler>();
            services.AddScoped<IRequestHandler<EditSalarioCommand, bool>, SalarioCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteSalarioCommand, bool>, SalarioCommandHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
        }
    }
}
