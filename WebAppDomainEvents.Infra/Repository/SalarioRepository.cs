using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task AdicionarSalarioAsync(Salario salario)
        {
            await _context.Set<Salario>().AddAsync(salario);
            await CommitAsync();
        }

        public async Task EditarSalarioAsync(Salario salario)
        {
            _context.Set<Salario>().Update(salario);
            await CommitAsync();
        }

        public async Task RemoverSalarioAsync(Salario salario)
        {
            _context.Set<Salario>().Update(salario);
            await CommitAsync();
        }

        public async Task<Salario> ObterSalarioPorIdAsync(Guid id) => await _context.Set<Salario>().FindAsync(id);

        public async Task<IReadOnlyCollection<Salario>> ObterSalarioAsync() => await _context.Set<Salario>().ToListAsync();
    }
}
