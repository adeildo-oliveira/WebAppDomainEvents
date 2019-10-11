using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;

namespace WebAppDomainEvents.Infra.Context
{
    public abstract class DomainEventsContextDapper
    {
        private static readonly string ObterConnectionString = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build()
            .GetConnectionString("ApiConnection");

        public static IDbConnection Connection => new SqlConnection(ObterConnectionString);
    }
}
