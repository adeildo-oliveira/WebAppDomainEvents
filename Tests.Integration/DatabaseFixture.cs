using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.DomainEvents;
using WebAppDomainEvents.Infra.Context;

namespace Tests.Integration
{
    public class DatabaseFixture : IDisposable
    {
        public readonly DbContext Context = new DomainEventsContext();
        public readonly HttpClient _client;
        private readonly TestServer server;

        public DatabaseFixture()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
        }

        public async Task<T> Criar<T>(T entity) where T : class
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        protected async Task<T> BuscarAsync<T>(Expression<Func<T, bool>> predicate) where T : class => 
            await Context.Set<T>().AsNoTracking().Where(predicate).FirstOrDefaultAsync();

        protected async Task<T> BuscarTodos<T>() where T : class =>
            await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync();

        public void ClearDataBase()
        {
            using (var db = new DomainEventsContext())
            {
                db.Database.Migrate();
                db.Database.ExecuteSqlCommand(Script);
            }
        }

        private static string Script => new StringBuilder(@" 
            ALTER TABLE Salario NOCHECK CONSTRAINT ALL
            ALTER TABLE DespesaMensal NOCHECK CONSTRAINT ALL
  
            delete from Salario
            delete from DespesaMensal

            ALTER TABLE Salario WITH CHECK CHECK CONSTRAINT ALL
            ALTER TABLE DespesaMensal WITH CHECK CHECK CONSTRAINT ALL").ToString();

        public void Dispose() => Context.Dispose();
    }
}
