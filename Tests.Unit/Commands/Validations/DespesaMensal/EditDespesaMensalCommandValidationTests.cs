using FluentAssertions;
using System;
using Tests.Shared.Builders.Commands;
using Xunit;

namespace Tests.Unit.Commands.Validations.DespesaMensal
{
    public class EditDespesaMensalCommandValidationTests
    {
        [Fact]
        public void EditDespesaMensalDeveValidarId()
        {
            var command = new EditDespesaMensalCommandBuilder()
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComDescricao("teste")
                .ComValor(decimal.One)
                .ComData(DateTime.Now)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Count.Should().Be(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id despesa mensal inválido");
        }

        [Fact]
        public void EditDespesaMensalDeveValidarIdSalario()
        {
            var command = new EditDespesaMensalCommandBuilder()
                .ComId(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComDescricao("Teste")
                .ComValor(decimal.One)
                .ComData(DateTime.Now)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Count.Should().Be(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id salário inválido");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EditDespesaMensalDeveValidarDescricao(string descricao)
        {
            var command = new EditDespesaMensalCommandBuilder()
                .ComId(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
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

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void EditDespesaMensalDeveValidarValor(decimal valor)
        {
            var command = new EditDespesaMensalCommandBuilder()
                .ComId(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
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
        public void EditDespesaMensalDeveValidarData()
        {
            var command = new EditDespesaMensalCommandBuilder()
                .ComId(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
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
        public void EditDespesaMensalNaoDeveApresentarMensagensValidacao()
        {
            var command = new EditDespesaMensalCommandBuilder()
                .ComId(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
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
