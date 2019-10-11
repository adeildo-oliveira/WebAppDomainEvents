using System.Threading.Tasks;
using WebAppDomainEvents.Infra.Context;

namespace WebAppDomainEvents.Infra.Repository
{
    public abstract class ContextSave
    {
        private readonly DomainEventsContext _context;

        protected ContextSave(DomainEventsContext context) => _context = context;

        public async Task CommitAsync() => await _context.SaveChangesAsync();

        public void Commit() => _context.SaveChanges();
    }
}
