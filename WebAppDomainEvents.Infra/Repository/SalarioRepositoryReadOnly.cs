using Dapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Infra.Context;
using WebAppDomainEvents.Infra.Repository.ScriptsReadOnly;

namespace WebAppDomainEvents.Infra.Repository
{
    public class SalarioRepositoryReadOnly : DomainEventsContextDapper, ISalarioRepositoryReadOnly
    {
        public SalarioRepositoryReadOnly(IHostingEnvironment env) : base(env) { }

        public async Task<IReadOnlyCollection<Salario>> ObterSalariosAsync()
        {
            using (var conn = Connection)
            {
                var result = await conn.QueryAsync<Salario>(ScriptsSalarioReadOnly.QueryObterSalario, commandType: CommandType.Text);

                return result.AsList();
            }
        }

        public async Task<Salario> ObterSalarioPorIdAsync(Guid id)
        {
            using (var conn = Connection)
            {
                return await conn.QueryFirstOrDefaultAsync<Salario>(ScriptsSalarioReadOnly.QueryObterSalarioPorId,
                    param: new
                    {
                        ID = id
                    }
                    , commandType: CommandType.Text);
            }
        }

        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
