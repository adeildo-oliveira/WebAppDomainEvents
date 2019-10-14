using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebAppDomainEvents.Domain.Interfaces.Repository
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        Task AddAsync(TEntity obj, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity obj, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity obj, CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
