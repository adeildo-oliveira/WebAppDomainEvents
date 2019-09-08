using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Interfaces.Repository
{
    public interface IDespesaMensalRepository
    {
        Task AdicionarDespesaMensalAsync(DespesaMensal despesaMensal);
        Task<DespesaMensal> ObterDespesaMensalPorIdAsync(Guid id);
        Task<IReadOnlyCollection<DespesaMensal>> ObterDespesasMensaisAsync();
        Task<IReadOnlyCollection<DespesaMensal>> ObterDespesasMensaisAsync(DespesaMensal despesaMensal);
        void AtualizarDespesaMensalPorId(DespesaMensal despesaMensal);
        void RemoverDespesaMensal(DespesaMensal despesaMensal);
    }
}
