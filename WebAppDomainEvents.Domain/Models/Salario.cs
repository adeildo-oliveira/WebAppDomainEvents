using System.Collections.Generic;

namespace WebAppDomainEvents.Domain.Models
{
    public class Salario : Entity
    {
        public Salario() { }

        public Salario(decimal pagamento, decimal adiantamento, bool status = true)
        {
            Pagamento = pagamento;
            Adiantamento = adiantamento;
            Status = status;
        }

        public decimal Pagamento { get; private set; }
        public decimal Adiantamento { get; private set; }
        public bool Status { get; private set; }
        public IEnumerable<DespesaMensal> DespesasMensais { get; private set; }
    }
}
