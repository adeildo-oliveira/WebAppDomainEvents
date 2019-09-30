using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using Xunit;

namespace Tests.Integration.Commands
{
    public class DomainEventsContextTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly ISalarioRepository _salarioRepository;
        private readonly IntegrationTestFixture _fixture;
        private readonly CancellationTokenSource source = new CancellationTokenSource();

        public DomainEventsContextTests(IntegrationTestFixture fixture)
        {
            Task.Delay(1000, source.Token).Wait();
            _fixture = fixture;
            _salarioRepository = _fixture.Service.GetService<ISalarioRepository>();
            _fixture.ClearDataBase();
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 54321.24)]
        [InlineData(12345.42, 1)]
        [InlineData(12345.42, 54321.24)]
        public async Task DeveInserirUmSalario(decimal pagamento, decimal adiantamento)
        {
            var salario = new Salario(pagamento, adiantamento);

            await _fixture.Criar(salario);
            var resultado = await _salarioRepository.ObterSalarioPorIdAsync(salario.Id);

            resultado.Should().NotBeNull();
            resultado.Pagamento.Should().Be(salario.Pagamento);
            resultado.Adiantamento.Should().Be(salario.Adiantamento);
            resultado.Status.Should().BeTrue();
        }

        [Theory]
        [InlineData("Cartão de Crédito", 1)]
        [InlineData("Mercado", 54321.22)]
        [InlineData("TV", 12345.85)]
        public async Task DeveInserirUmaDespesaMensal(string descricao, decimal valor)
        {
            var data = DateTime.Now;
            var salario = new Salario(12345.55M, 54321.56M);
            var despesaMensal = new DespesaMensal(descricao, valor, data)
                .AdicionarSalario(salario);

            await _fixture.Criar(despesaMensal);

            var resultado = await _salarioRepository.ObterSalarioPorIdAsync(salario.Id);
            var resultadoDespesaMensal = resultado.DespesasMensais.FirstOrDefault();

            resultado.Should().NotBeNull();
            resultadoDespesaMensal.Descricao.Should().Be(despesaMensal.Descricao);
            resultadoDespesaMensal.Valor.Should().Be(despesaMensal.Valor);
            resultadoDespesaMensal.Data.Date.Should().Be(despesaMensal.Data.Date);
            resultadoDespesaMensal.Status.Should().BeTrue();
        }

        [Fact]
        public async Task DeveConsultarSalarioPorId()
        {
            var salario = new Salario(12345.55M, 54321.56M);
            await _fixture.Criar(salario);

            var resultado = await _salarioRepository.ObterSalarioPorIdAsync(salario.Id);
            
            resultado.Should().NotBeNull();
            salario.Should().BeEquivalentTo(resultado);
        }

        [Fact]
        public async Task DeveConsultarSalarios()
        {
            var salario1 = new Salario(2857.02M, 3178.62M);
            var salario2 = new Salario(2857.00M, 3178.00M);
            var salario3 = new Salario(12345.55M, 54321.56M);
            await _fixture.Criar(salario1);
            await _fixture.Criar(salario2);
            await _fixture.Criar(salario3);

            var resultado = await _salarioRepository.ObterSalarioAsync();
            
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
        }
    }
}
