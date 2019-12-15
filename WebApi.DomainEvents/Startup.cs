using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using WebAppDomainEvents.CrossCutting;
using WebAppDomainEvents.Domain.Interfaces.Repository.Logue;
using WebAppDomainEvents.Infra.Repository.Logue;

namespace WebApi.DomainEvents
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment environment)
        {
            _environment = environment;

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.Load("WebApi.DomainEvents"));
            services.AddSingleton<ILogger>(x => new LoggerConfiguration().ReadFrom.Configuration(_configuration).CreateLogger());

            services.AddCors(o => o.AddPolicy("Local", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                       .AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            MongoDbConfiguration(services);
            
            services.RegisterServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "POC Domain Events",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "teste@noemail.com",
                        Name = "Equipe interna"
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors("Local");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }

        private void MongoDbConfiguration(IServiceCollection services)
        {
            services.Configure<LogueDatabaseSettings>(_configuration.GetSection("MongoConnectionString"));
            services.AddSingleton<ILogueDatabaseSettings>(sp => sp.GetRequiredService<IOptions<LogueDatabaseSettings>>().Value);
            services.AddSingleton(typeof(ILogueMongodb<>), typeof(LogueMongodb<>));
        }
    }
}
