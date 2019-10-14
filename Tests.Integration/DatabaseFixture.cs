using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApi.DomainEvents;
using WebAppDomainEvents.Infra.Context;

namespace Tests.Integration
{
    public class DatabaseFixture : IDisposable
    {
        public readonly DbContext Context;
        public readonly TestServer Server;

        public DatabaseFixture()
        {
            Server = Server ?? new TestServer(new WebHostBuilder().UseStartup<Startup>().UseEnvironment("Development"));
            Context = Context ?? new DomainEventsContext(Server.Services.GetService<IHostingEnvironment>());
        }

        public async Task<T> CriarAsync<T>(T entity) where T : class
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> BuscarAsync<T>(Expression<Func<T, bool>> predicate) where T : class => 
            await Context.Set<T>().AsNoTracking().Where(predicate).FirstOrDefaultAsync();

        public void ClearDataBase()
        {
            Context.Database.Migrate();
            Context.Database.ExecuteSqlCommand(Script);
        }

        private static string Script => new StringBuilder(@" 
            ALTER TABLE Salario NOCHECK CONSTRAINT ALL
            ALTER TABLE DespesaMensal NOCHECK CONSTRAINT ALL
  
            delete from Salario
            delete from DespesaMensal

            ALTER TABLE Salario WITH CHECK CHECK CONSTRAINT ALL
            ALTER TABLE DespesaMensal WITH CHECK CHECK CONSTRAINT ALL").ToString();

        public void Dispose()
        {
            Context.Dispose();
            Server.Dispose();
        }
    }
}
