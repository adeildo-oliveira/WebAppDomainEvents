using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Interfaces.Repository
{
    public interface IDespesaMensalRepositoryReadOnly : IDisposable
    {
        Task<DespesaMensal> ObterDespesaMensalPorIdAsync(Guid id);
        Task<IReadOnlyCollection<DespesaMensal>> ObterDespesasMensaisAsync();
    }
}
