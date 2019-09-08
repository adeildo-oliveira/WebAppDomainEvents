using FluentAssertions;
using System;
using Tests.Shared.Builders.Commands;
using Xunit;

namespace Tests.Unit.Commands.Validations.Salario
{
    public class EditSalarioCommandValidationTests
    {
        [Fact]
        public void DeveValidarId()
        {
            var command = new EditSalarioCommandBuilder()
                .ComAdiantamento(decimal.One)
                .ComPagamento(decimal.One)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id salário inválido");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void DeveValidarPagamento(decimal valor)
        {
            var command = new EditSalarioCommandBuilder()
                .ComId(Guid.NewGuid())
                .ComAdiantamento(decimal.One)
                .ComPagamento(valor)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("O valor Pagamento deve ser maior que zero");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void DeveValidarAdiantamento(decimal valor)
        {
            var command = new EditSalarioCommandBuilder()
                .ComId(Guid.NewGuid())
                .ComAdiantamento(valor)
                .ComPagamento(decimal.One)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("O valor Adiantamento deve ser maior que zero");
        }
    }
}
