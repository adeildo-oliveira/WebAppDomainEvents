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
        public IReadOnlyCollection<DespesaMensalSalarioView> DespesasMensais { get; set; }
    }

    public class DespesaMensalSalarioView
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public bool Status { get; set; }
    }
}
