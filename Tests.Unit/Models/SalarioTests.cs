using FluentAssertions;
using System;
using System.Linq;
using Tests.Shared.Builders.Models;
using WebAppDomainEvents.Domain.Models;
using Xunit;

namespace Tests.Unit.Models
{
    public class SalarioTests
    {
        [Fact]
        public void DeveAtualizarDadosDoSalario()
        {
            const decimal pagamento = 12.32M;
            const decimal adiantamento = 20M;
            const bool status = true;

            var id = new Guid("32cd6820-0da5-4c5f-94d1-e73b01f05de2");

            var salarioBuilder = new SalarioBuilder()
                .ComPagamento(pagamento)
                .ComAdiantamento(adiantamento)
                .Instanciar();

            salarioBuilder.AtualizarId(id);
            salarioBuilder.AtualizarStatus(status);

            salarioBuilder.Id.Should().Be(id);
            salarioBuilder.Pagamento.Should().Be(pagamento);
            salarioBuilder.Adiantamento.Should().Be(adiantamento);
            salarioBuilder.Status.Should().BeTrue();
        }

        [Fact]
        public void DeveAtualizarDadosDoSalarioComStatusFalso()
        {
            const decimal pagamento = 12.32M;
            const decimal adiantamento = 20M;
            const bool status = false;

            var id = new Guid("32cd6820-0da5-4c5f-94d1-e73b01f05de2");

            var salarioBuilder = new SalarioBuilder()
                .ComPagamento(pagamento)
                .ComAdiantamento(adiantamento)
                .Instanciar();

            salarioBuilder.AtualizarId(id);
            salarioBuilder.AtualizarStatus(status);

            salarioBuilder.Id.Should().Be(id);
            salarioBuilder.Pagamento.Should().Be(pagamento);
            salarioBuilder.Adiantamento.Should().Be(adiantamento);
            salarioBuilder.Status.Should().BeFalse();
        }

        [Fact]
        public void AtualizarDadosDespesaMenal()
        {
            var data = DateTime.Now;
            var despesaMensal = new DespesaMensal("Cartão", 22.55M, new DateTime(2019, 09, 05));
            var despesaMensal2 = new DespesaMensal("Celular", 32.99M, new DateTime(2019, 09, 05));
            var salario = new Salario(1200.22M, 1500.45M)
                .AdicionarDespesaMensal(despesaMensal)
                .AdicionarDespesaMensal(despesaMensal2);
            
            var result = salario.DespesasMensais.FirstOrDefault(x => x.Id == despesaMensal2.Id);
            result.AtualizarDespesaMensal("NetFlix", 45.00M, data);

            salario.DespesasMensais.Should().HaveCount(2);
            salario.Id.Should().NotBeEmpty();
            salario.Pagamento.Should().Be(1200.22M);
            salario.Adiantamento.Should().Be(1500.45M);
            salario.Status.Should().BeTrue();

            salario.DespesasMensais.FirstOrDefault(x => x.Id == result.Id).Descricao.Should().Be("NetFlix");
            salario.DespesasMensais.FirstOrDefault(x => x.Id == result.Id).Valor.Should().Be(45.00M);
            salario.DespesasMensais.FirstOrDefault(x => x.Id == result.Id).Data.Should().Be(data);

            salario.DespesasMensais.FirstOrDefault(x => x.Id == despesaMensal.Id).Descricao.Should().Be("Cartão");
            salario.DespesasMensais.FirstOrDefault(x => x.Id == despesaMensal.Id).Valor.Should().Be(22.55M);
            salario.DespesasMensais.FirstOrDefault(x => x.Id == despesaMensal.Id).Data.Should().Be(new DateTime(2019, 09, 05));
        }
    }
}
