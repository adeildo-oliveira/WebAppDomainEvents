using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Interfaces.Repository
{
    public interface ISalarioRepository
    {
        Task AdicionarSalarioAsync(Salario salario);
        Task<Salario> ObterSalarioPorIdAsync(Guid id);
        Task<IReadOnlyCollection<Salario>> ObterSalarioAsync();
        Task EditarSalarioAsync(Salario salario);
        Task RemoverSalarioAsync(Salario salario);
    }
}
