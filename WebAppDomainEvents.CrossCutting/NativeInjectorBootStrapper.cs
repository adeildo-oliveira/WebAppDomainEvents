using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebAppDomainEvents.Infra.Context;

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

        private static void InfraServices(IServiceCollection services) =>
            services.AddScoped<DomainEventsContext>();

        private static void DomainServices(IServiceCollection services)
        {
            
        }
    }
}
