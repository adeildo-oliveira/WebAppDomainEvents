using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Interfaces.Repository
{
    public interface ISalarioRepository : IDisposable
    {
        Task AdicionarSalarioAsync(Salario salario);
        Task EditarSalarioAsync(Salario salario);
        Task RemoverSalarioAsync(Salario salario);
        Task<Salario> ObterSalarioPorIdAsync(Guid id);
        Task<IReadOnlyCollection<Salario>> ObterSalarioAsync();
    }
}
