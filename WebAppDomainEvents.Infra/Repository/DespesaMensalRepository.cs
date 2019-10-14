using ProjetoDespesasMensais.Infra.Data.Repository;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Infra.Context;

namespace WebAppDomainEvents.Infra.Repository
{
    public class DespesaMensalRepository : RepositoryBase<DespesaMensal>, IDespesaMensalRepository
    {
        public DespesaMensalRepository(DomainEventsContext context) : base(context)
        {
        }
    }
}
