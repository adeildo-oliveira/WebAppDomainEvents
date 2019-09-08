using FluentAssertions;
using System;
using Tests.Shared.Builders.Commands;
using Xunit;

namespace Tests.Unit.Commands.Validations.Salario
{
    public class DeleteSalarioCommandValidationTests
    {
        [Fact]
        public void DeveValidarIdSalario()
        {
            var command = new DeleteSalarioCommandBuilder()
                .ComStatus(false)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCount(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id salário inválido");
        }

        [Fact]
        public void DeveValidarStatusSalario()
        {
            var command = new DeleteSalarioCommandBuilder()
                .ComId(Guid.NewGuid())
                .ComStatus(true)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCount(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("O campo status deve ser informado");
        }

        [Fact]
        public void DeveValidarIdEStatusSalario()
        {
            var command = new DeleteSalarioCommandBuilder()
                .ComStatus(true)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCount(2);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id salário inválido");
            command.ValidationResult.Errors[1].ErrorMessage.Should().Be("O campo status deve ser informado");
        }

        [Fact]
        public void NaoDeveApresentarMensagemValidacaoSalario()
        {
            var command = new DeleteSalarioCommandBuilder()
                .ComId(Guid.NewGuid())
                .ComStatus(false)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.Errors.Should().HaveCount(0);
            command.ValidationResult.IsValid.Should().BeTrue();
        }
    }
}
