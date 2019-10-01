using FluentAssertions;
using System;
using Tests.Shared.Builders.Commands;
using Xunit;

namespace Tests.Unit.Commands.Validations.DespesaMensal
{
    public class DeleteDespesaMensalCommandValidationTests
    {
        [Fact]
        public void DeveValidarIdDespesaMensal()
        {
            var command = new DeleteDespesaMensalCommandBuilder().Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id despesa mensal inválido");
        }

        [Fact]
        public void DeveValidarStatusDespesaMensal()
        {
            var command = new DeleteDespesaMensalCommandBuilder()
                .ComId(Guid.NewGuid())
                .ComStatus(true)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Campo status inválido para exclusão");
        }

        [Fact]
        public void DeveValidarIdEStatusDespesaMensal()
        {
            var command = new DeleteDespesaMensalCommandBuilder()
                .ComStatus(true)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCount(2);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id despesa mensal inválido");
            command.ValidationResult.Errors[1].ErrorMessage.Should().Be("Campo status inválido para exclusão");
        }

        [Fact]
        public void NaoDeveApresentarMensagemValidacaoDespesaMensal()
        {
            var command = new DeleteDespesaMensalCommandBuilder()
                .ComId(Guid.NewGuid())
                .ComStatus(false)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeTrue();
        }
    }
}
