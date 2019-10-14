using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using Xunit;

namespace Tests.Integration.DomainEventsReadOnly
{
    public class SalarioRepositoryReadOnlyTests : IntegrationTestFixture
    {
        private readonly ISalarioRepositoryReadOnly _salarioRepository;
        private readonly ISalarioRepository _repository;

        public SalarioRepositoryReadOnlyTests(DatabaseFixture fixture) : base(fixture)
        {
            _salarioRepository = _fixture.Server.Services.GetService<ISalarioRepositoryReadOnly>();
            _repository = _fixture.Server.Services.GetService<ISalarioRepository>();
        }

        [Fact]
        public async Task ObterSalariosReadOnly()
        {
            var salario1 = new Salario(1500.55M, 3200.89M);
            var salario2 = new Salario(1800.55M, 4000.89M);
            var salario3 = new Salario(2000.55M, 5000.89M);

            await _repository.AddAsync(salario1);
            await _repository.AddAsync(salario2);
            await _repository.AddAsync(salario3);

            var resultado = await _salarioRepository.ObterSalariosAsync();

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(3);
            resultado.FirstOrDefault(x => x.Id == salario1.Id).Should().BeEquivalentTo(salario1);
            resultado.FirstOrDefault(x => x.Id == salario2.Id).Should().BeEquivalentTo(salario2);
            resultado.FirstOrDefault(x => x.Id == salario3.Id).Should().BeEquivalentTo(salario3);
        }

        [Fact]
        public async Task ObterSalarioPorIdReadOnly()
        {
            var salario1 = new Salario(1500.55M, 3200.89M);
            var salario2 = new Salario(1800.55M, 4000.89M);
            var salario3 = new Salario(2000.55M, 5000.89M);

            await _repository.AddAsync(salario1);
            await _repository.AddAsync(salario2);
            await _repository.AddAsync(salario3);

            var resultado = await _salarioRepository.ObterSalarioPorIdAsync(salario1.Id);

            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(salario1);
        }
    }
}
