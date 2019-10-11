using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAppDomainEvents.Domain.Models
{
    public class Salario : Entity
    {
        protected Salario() { }

        public Salario(decimal pagamento, decimal adiantamento)
        {
            Pagamento = pagamento;
            Adiantamento = adiantamento;
        }

        public Salario(Guid id, decimal pagamento, decimal adiantamento, bool status)
        {
            Id = id;
            Pagamento = pagamento;
            Adiantamento = adiantamento;
            Status = status;
        }

        public decimal Pagamento { get; private set; }
        public decimal Adiantamento { get; private set; }
        public bool Status { get; private set; } = true;
        public ICollection<DespesaMensal> DespesasMensais { get; private set; }

        public virtual Salario AtualizarId(Guid id)
        {
            Id = id;
            return this;
        }

        public virtual Salario AtualizarStatus(bool status)
        {
            Status = status;
            return this;
        }

        public virtual Salario AdicionarDespesaMensal(DespesaMensal despesaMensal)
        {
            DespesasMensais = DespesasMensais ?? new List<DespesaMensal>();

            if (!DespesasMensais.Any(x => x.Id == despesaMensal?.Id))
                DespesasMensais.Add(despesaMensal);

            return this;
        }
    }
}
