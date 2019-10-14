using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;

namespace WebAppDomainEvents.Infra.Context
{
    public abstract class DomainEventsContextDapper
    {
        private readonly IHostingEnvironment _env;

        public DomainEventsContextDapper(IHostingEnvironment env) => _env = env;

        private string ObterConnectionString => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(_env.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json", optional: true, reloadOnChange: true)
            .Build()
            .GetConnectionString("ApiConnection");

        public IDbConnection Connection => new SqlConnection(ObterConnectionString);
    }
}
