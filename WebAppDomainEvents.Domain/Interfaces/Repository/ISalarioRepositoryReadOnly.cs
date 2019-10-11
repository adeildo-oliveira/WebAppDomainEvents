using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Models;

namespace WebAppDomainEvents.Domain.Interfaces.Repository
{
    public interface ISalarioRepositoryReadOnly : IDisposable
    {
        Task<Salario> ObterSalarioPorIdAsync(Guid id);
        Task<IReadOnlyCollection<Salario>> ObterSalariosAsync();
    }
}
