using System;
using System.Collections.Generic;

namespace WebAppDomainEvents.Domain.Models
{
    public class Salario : Entity
    {
        public Salario() { }

        public Salario(decimal pagamento, decimal adiantamento)
        {
            Pagamento = pagamento;
            Adiantamento = adiantamento;
        }

        public decimal Pagamento { get; private set; }
        public decimal Adiantamento { get; private set; }
        public bool Status { get; private set; } = true;
        public IEnumerable<DespesaMensal> DespesasMensais { get; private set; }

        public Salario AtualizarId(Guid id)
        {
            Id = id;
            return this;
        }

        public Salario AtualizarStatus(bool status)
        {
            Status = status;
            return this;
        }
    }
}
