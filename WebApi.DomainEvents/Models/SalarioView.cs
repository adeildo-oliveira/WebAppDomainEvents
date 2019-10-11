using System;
using System.Collections.Generic;

namespace WebApi.DomainEvents.Models
{
    public class SalarioView
    {
        public Guid Id { get; set; }
        public decimal Pagamento { get; set; }
        public decimal Adiantamento { get; set; }
        public bool Status { get; set; }
        public IReadOnlyCollection<DespesaMensalView> DespesasMensaisView { get; set; }
    }
}
