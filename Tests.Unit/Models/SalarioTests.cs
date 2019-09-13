using FluentAssertions;
using System;
using Tests.Shared.Builders.Models;
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
    }
}
