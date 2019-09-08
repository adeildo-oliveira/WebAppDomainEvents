using FluentAssertions;
using Tests.Shared.Builders.Commands;
using Xunit;

namespace Tests.Unit.Commands.Validations.Salario
{
    public class AddSalarioCommandValidationTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void DeveValidarPagamento(decimal valor)
        {
            var command = new AddSalarioCommandBuilder()
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
            var command = new AddSalarioCommandBuilder()
                .ComAdiantamento(valor)
                .ComPagamento(decimal.One)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("O valor Adiantamento deve ser maior que zero");
        }

        [Fact]
        public void NaoDeveApresentarMensagensValidacao()
        {
            var command = new AddSalarioCommandBuilder()
                .ComAdiantamento(decimal.One)
                .ComPagamento(decimal.One)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeTrue();
            command.ValidationResult.Errors.Count.Should().BeLessThan(1);
        }
    }
}
