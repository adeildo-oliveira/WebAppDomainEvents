using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Infra.Context;

namespace WebAppDomainEvents.Infra.Repository
{
    public class SalarioRepository : ContextSave, ISalarioRepository
    {
        private readonly DomainEventsContext _context;

        public SalarioRepository(DomainEventsContext context) : base(context) => _context = context;

        public virtual async Task AdicionarSalarioAsync(Salario salario)
        {
            await _context.Set<Salario>().AddAsync(salario);
            await CommitAsync();
        }

        public virtual async Task EditarSalarioAsync(Salario salario)
        {
            _context.Attach(salario);
            _context.Set<Salario>().Update(salario).State = EntityState.Modified;
            await CommitAsync();
        }

        public virtual async Task RemoverSalarioAsync(Salario salario)
        {
            _context.Set<Salario>().Update(salario);
            await CommitAsync();
        }

        public virtual async Task<Salario> ObterSalarioPorIdAsync(Guid id) => await _context.Set<Salario>()
            .Include(x => x.DespesasMensais)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.Status == true);

        public virtual async Task<IReadOnlyCollection<Salario>> ObterSalarioAsync() => 
            await _context.Set<Salario>()
            .Include(x => x.DespesasMensais)
            .Where(x => x.Status == true)
            .AsNoTracking()
            .ToListAsync();

        public void Dispose() => _context.Dispose();
    }
}
