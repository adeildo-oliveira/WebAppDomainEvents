using FluentAssertions;
using System;
using Tests.Shared.Builders.Commands;
using Xunit;

namespace Tests.Unit.Commands.Validations.DespesaMensal
{
    public class AddDespesaMensalCommandValidationTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AddDespesaMensalDeveValidarDescricao(string descricao)
        {
            var command = new AddDespesaMensalCommandBuilder()
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComDescricao(descricao)
                .ComValor(decimal.One)
                .ComData(DateTime.Now)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Count.Should().Be(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Descrição inválida");
        }

        [Fact]
        public void AddDespesaMensalDeveValidarIdSalario()
        {
            var command = new AddDespesaMensalCommandBuilder()
                .ComDescricao("teste")
                .ComValor(decimal.One)
                .ComData(DateTime.Now)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Count.Should().Be(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id salário inválido");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void AddDespesaMensalDeveValidarValor(decimal valor)
        {
            var command = new AddDespesaMensalCommandBuilder()
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComDescricao("Teste")
                .ComValor(valor)
                .ComData(DateTime.Now)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Count.Should().Be(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("O valor Pagamento deve ser maior que zero");
        }

        [Fact]
        public void AddDespesaMensalDeveValidarData()
        {
            var command = new AddDespesaMensalCommandBuilder()
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComDescricao("Teste")
                .ComValor(decimal.One)
                .ComData(DateTime.MinValue)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Count.Should().Be(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Data inválida");
        }

        [Fact]
        public void AddDespesaMensalNaoDeveApresentarMensagensValidacao()
        {
            var command = new AddDespesaMensalCommandBuilder()
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComDescricao("Teste")
                .ComValor(decimal.One)
                .ComData(DateTime.Now)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeTrue();
            command.ValidationResult.Errors.Count.Should().BeLessThan(1);
        }
    }
}
