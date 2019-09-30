using System;
using System.Collections.Generic;

namespace WebApi.DomainEvents.Models
{
    public class SalarioView
    {
        public Guid Id { get; set; }
        public decimal Pagamento { get; private set; }
        public decimal Adiantamento { get; private set; }
        public bool Status { get; private set; } = true;
        public IReadOnlyCollection<DespesaMensalView> DespesasMensaisView { get; private set; }
    }
}
