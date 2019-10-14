using Microsoft.EntityFrameworkCore;
using ProjetoDespesasMensais.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Infra.Context;

namespace WebAppDomainEvents.Infra.Repository
{
    public class SalarioRepository : RepositoryBase<Salario>, ISalarioRepository
    {
        public SalarioRepository(DomainEventsContext context) : base(context)
        {
        }

        public override async Task<Salario> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) 
            => await DbSet
            .AsNoTracking()
            .Include(x => x.DespesasMensais)
            .FirstOrDefaultAsync(x => x.Id == id && x.Status == true, cancellationToken);

        public override async Task<IReadOnlyCollection<Salario>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await DbSet.AsNoTracking().Include(x => x.DespesasMensais).Where(x => x.Status == true).ToListAsync(cancellationToken);
    }
}
