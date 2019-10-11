using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Infra.Context;
using WebAppDomainEvents.Infra.Repository.ScriptsReadOnly;

namespace WebAppDomainEvents.Infra.Repository
{
    public class DespesaMensalRepositoryReadOnly : DomainEventsContextDapper, IDespesaMensalRepositoryReadOnly
    {
        public async Task<DespesaMensal> ObterDespesaMensalPorIdAsync(Guid id)
        {
            using (var conn = Connection)
            {
                var result = await conn.QueryAsync<DespesaMensal, Salario, DespesaMensal>(ScriptsDespesaMensalReadOnly.QueryObterDespesaMensalPorId,
                    (despesaMensal, salario) =>
                    {
                        despesaMensal.AdicionarSalario(salario);
                        return despesaMensal;
                    }
                    , param: new { ID = id }
                    , splitOn: "Id"
                    , commandType: CommandType.Text);

                return result.FirstOrDefault();
            }
        }

        public async Task<IReadOnlyCollection<DespesaMensal>> ObterDespesasMensaisAsync()
        {
            using (var conn = Connection)
            {
                var result = await conn.QueryAsync<DespesaMensal, Salario, DespesaMensal>(ScriptsDespesaMensalReadOnly.QueryObterDespesaMensal,
                    (despesaMensal, salario) =>
                    {
                        despesaMensal.AdicionarSalario(salario);
                        return despesaMensal;
                    }
                    , splitOn: "Id"
                    , commandType: CommandType.Text);

                return result.AsList();
            }
        }

        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
