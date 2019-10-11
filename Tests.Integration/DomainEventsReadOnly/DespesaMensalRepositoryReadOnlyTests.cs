using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using Xunit;

namespace Tests.Integration.DomainEventsReadOnly
{
    public class DespesaMensalRepositoryReadOnlyTests : IntegrationTestFixture
    {
        private readonly IDespesaMensalRepositoryReadOnly _despesaMensalRepository;

        public DespesaMensalRepositoryReadOnlyTests(DatabaseFixture fixture) : base(fixture) => 
            _despesaMensalRepository = Service.GetService<IDespesaMensalRepositoryReadOnly>();

        [Fact]
        public async Task ObterDespesasMensaisRepositoryReadOnly()
        {
            var despesaMensal1 = new DespesaMensal("Cartão de Crédito", 444.05M, new DateTime(2019, 10, 6));
            var despesaMensal2 = new DespesaMensal("Celular Petit", 29.99M, new DateTime(2019, 10, 6));
            var despesaMensal3 = new DespesaMensal("Celular", 32.99M, new DateTime(2019, 10, 6));

            await _fixture.CriarAsync(new Salario(1500.55M, 3200.89M).AdicionarDespesaMensal(despesaMensal1));
            await _fixture.CriarAsync(new Salario(1800.55M, 4000.89M).AdicionarDespesaMensal(despesaMensal2));
            await _fixture.CriarAsync(new Salario(2000.55M, 5000.89M).AdicionarDespesaMensal(despesaMensal3));

            var resultado = await _despesaMensalRepository.ObterDespesasMensaisAsync();

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.FirstOrDefault(x => x.Id == despesaMensal1.Id && x.Descricao == despesaMensal1.Descricao).Should().NotBeNull();
            resultado.FirstOrDefault(x => x.Id == despesaMensal2.Id && x.Descricao == despesaMensal2.Descricao).Should().NotBeNull();
            resultado.FirstOrDefault(x => x.Id == despesaMensal3.Id && x.Descricao == despesaMensal3.Descricao).Should().NotBeNull();
        }

        [Fact]
        public async Task ObterDespesaMensalRepositoryPorIdReadOnly()
        {
            var despesaMensal1 = new DespesaMensal("Cartão de Crédito", 444.05M, new DateTime(2019, 10, 6));
            var despesaMensal2 = new DespesaMensal("Celular Petit", 29.99M, new DateTime(2019, 10, 6));
            var despesaMensal3 = new DespesaMensal("Celular", 32.99M, new DateTime(2019, 10, 6));

            await _fixture.CriarAsync(new Salario(1500.55M, 3200.89M).AdicionarDespesaMensal(despesaMensal1));
            await _fixture.CriarAsync(new Salario(1800.55M, 4000.89M).AdicionarDespesaMensal(despesaMensal2));
            await _fixture.CriarAsync(new Salario(2000.55M, 5000.89M).AdicionarDespesaMensal(despesaMensal3));

            var resultado = await _despesaMensalRepository.ObterDespesaMensalPorIdAsync(despesaMensal2.Id);

            resultado.Should().NotBeNull();
            resultado.Id.Should().Be(despesaMensal2.Id);
            resultado.Descricao.Should().Be(despesaMensal2.Descricao);
            resultado.Valor.Should().Be(despesaMensal2.Valor);
            resultado.Data.Should().Be(despesaMensal2.Data);
            resultado.Status.Should().Be(despesaMensal2.Status);
        }
    }
}
