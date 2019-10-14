using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Infra.Context;

namespace ProjetoDespesasMensais.Infra.Data.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected DomainEventsContext Context;
        protected DbSet<TEntity> DbSet;

        protected RepositoryBase(DomainEventsContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity obj, CancellationToken cancellationToken = default)
        {
            DbSet.Attach(obj);
            await DbSet.AddAsync(obj);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity obj, CancellationToken cancellationToken = default)
        {
            DbSet.Attach(obj);
            DbSet.Update(obj).State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity obj, CancellationToken cancellationToken = default)
        {
            DbSet.Attach(obj);
            DbSet.Update(obj).State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
            => await DbSet.FindAsync(id, cancellationToken);

        public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
