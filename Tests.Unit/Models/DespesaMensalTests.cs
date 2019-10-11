using FluentAssertions;
using System;
using WebAppDomainEvents.Domain.Models;
using Xunit;

namespace Tests.Unit.Models
{
    public class DespesaMensalTests
    {
        [Fact]
        public void DespesaMensalAdicionarSalario()
        {
            var data = DateTime.Now;
            var salario = new Salario(decimal.One, decimal.One);
            var despesaMensal = new DespesaMensal("Cartão", 22.55M, data)
                .AdicionarSalario(salario);

            despesaMensal.Should().NotBeNull();
            despesaMensal.Id.Should().NotBeEmpty();
            despesaMensal.Descricao.Should().Be("Cartão");
            despesaMensal.Valor.Should().Be(22.55M);
            despesaMensal.Data.Should().Be(data);
            despesaMensal.Status.Should().BeTrue();
            despesaMensal.Salario.Should().BeEquivalentTo(salario);
        }

        [Fact]
        public void DespesaMensalAtualizarDespesaMensal()
        {
            var data = DateTime.Now;
            var despesaMensal = new DespesaMensal("Cartão", 22.55M, DateTime.Now)
                .AtualizarDespesaMensal("Teste", 45.22M, data);

            despesaMensal.Should().NotBeNull();
            despesaMensal.Id.Should().NotBeEmpty();
            despesaMensal.Descricao.Should().Be("Teste");
            despesaMensal.Valor.Should().Be(45.22M);
            despesaMensal.Data.Should().Be(data);
            despesaMensal.Status.Should().BeTrue();
        }

        [Fact]
        public void DespesaMensalAtualizarDespesaMensalStatusParaFalso()
        {
            var data = DateTime.Now;
            var despesaMensal = new DespesaMensal("Cartão", 22.55M, data)
                .AtualizarDespesaMensal(false);

            despesaMensal.Should().NotBeNull();
            despesaMensal.Id.Should().NotBeEmpty();
            despesaMensal.Descricao.Should().Be("Cartão");
            despesaMensal.Valor.Should().Be(22.55M);
            despesaMensal.Data.Should().Be(data);
            despesaMensal.Status.Should().BeFalse();
        }

        [Fact]
        public void DespesaMensalAtualizarDespesaMensalStatusParaVerdadeiro()
        {
            var data = DateTime.Now;
            var despesaMensal = new DespesaMensal("Cartão", 22.55M, data)
                .AtualizarDespesaMensal(true);

            despesaMensal.Should().NotBeNull();
            despesaMensal.Id.Should().NotBeEmpty();
            despesaMensal.Descricao.Should().Be("Cartão");
            despesaMensal.Valor.Should().Be(22.55M);
            despesaMensal.Data.Should().Be(data);
            despesaMensal.Status.Should().BeTrue();
        }
    }
}
