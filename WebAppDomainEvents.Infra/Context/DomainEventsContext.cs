using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Infra.EntityConfig;

namespace WebAppDomainEvents.Infra.Context
{
    public class DomainEventsContext : DbContext
    {
        private readonly IHostingEnvironment _env;
        public DbSet<Salario> Salario { get; set; }
        public DbSet<DespesaMensal> DespesaMensal { get; set; }

        public DomainEventsContext() { }

        public DomainEventsContext(IHostingEnvironment env) => _env = env;

        public DomainEventsContext(DbContextOptions<DomainEventsContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new SalarioConfiguration());
            modelBuilder.ApplyConfiguration(new DespesaMensalConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_env.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json", optional: true, reloadOnChange: true)
                .Build()
                .GetConnectionString("ApiConnection"));
        }
    }
}
