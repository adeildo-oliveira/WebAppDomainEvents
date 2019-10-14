using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using Xunit;

namespace Tests.Integration.Commands
{
    public class DomainEventsContextTests : IntegrationTestFixture
    {
        private readonly ISalarioRepository _salarioRepository;

        public DomainEventsContextTests(DatabaseFixture fixture) : base(fixture) => _salarioRepository = _fixture.Server.Services.GetService<ISalarioRepository>();

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 54321.24)]
        [InlineData(12345.42, 1)]
        [InlineData(12345.42, 54321.24)]
        public async Task DeveInserirUmSalario(decimal pagamento, decimal adiantamento)
        {
            var salario = new Salario(pagamento, adiantamento);

            await _fixture.CriarAsync(salario);
            var resultado = await _salarioRepository.GetByIdAsync(salario.Id);

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

            await _fixture.CriarAsync(despesaMensal);

            var resultado = await _salarioRepository.GetByIdAsync(salario.Id);
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

            await _fixture.CriarAsync(salario);

            var resultado = await _salarioRepository.GetByIdAsync(salario.Id);
            
            resultado.Should().NotBeNull();
            salario.Id.Should().Be(resultado.Id);
            salario.Pagamento.Should().Be(resultado.Pagamento);
            salario.Adiantamento.Should().Be(resultado.Adiantamento);
            salario.Status.Should().Be(resultado.Status);
        }

        [Fact]
        public async Task DeveConsultarSalarios()
        {
            var salario1 = new Salario(2857.02M, 3178.62M);
            var salario2 = new Salario(2857.00M, 3178.00M);
            var salario3 = new Salario(12345.55M, 54321.56M);
            await _fixture.CriarAsync(salario1);
            await _fixture.CriarAsync(salario2);
            await _fixture.CriarAsync(salario3);

            var resultado = await _salarioRepository.GetAllAsync();
            
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
        }
    }
}
