using WebAppDomainEvents.Domain.Models;

namespace Tests.Shared.Builders.Models
{
    public class SalarioBuilder : InMemoryBuilder<Salario>
    {
        private decimal _pagamento;
        private decimal _adiantamento;

        public SalarioBuilder ComPagamento(decimal pagamento)
        {
            _pagamento = pagamento;
            return this;
        }

        public SalarioBuilder ComAdiantamento(decimal adiantamento)
        {
            _adiantamento = adiantamento;
            return this;
        }

        public override Salario Instanciar() => new Salario(_pagamento, _adiantamento);
    }
}
