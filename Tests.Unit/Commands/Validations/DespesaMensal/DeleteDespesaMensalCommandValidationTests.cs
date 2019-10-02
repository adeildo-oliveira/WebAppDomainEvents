using FluentAssertions;
using System;
using Tests.Shared.Builders.Commands;
using Xunit;

namespace Tests.Unit.Commands.Validations.DespesaMensal
{
    public class DeleteDespesaMensalCommandValidationTests
    {
        [Fact]
        public void DeleteDespesaMensalDeveValidarIdDespesaMensal()
        {
            var command = new DeleteDespesaMensalCommandBuilder()
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id despesa mensal inválido");
        }

        [Fact]
        public void DeleteDespesaMensalDeveValidarIdSalario()
        {
            var command = new DeleteDespesaMensalCommandBuilder()
                .ComId(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Count.Should().Be(1);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id salário inválido");
        }

        [Fact]
        public void DeleteDespesaMensalDeveValidarStatusDespesaMensal()
        {
            var command = new DeleteDespesaMensalCommandBuilder()
                .ComId(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComStatus(true)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Campo status inválido para exclusão");
        }

        [Fact]
        public void DeleteDespesaMensalDeveValidarIdEStatusDespesaMensal()
        {
            var command = new DeleteDespesaMensalCommandBuilder()
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComStatus(true)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCount(2);
            command.ValidationResult.Errors[0].ErrorMessage.Should().Be("Id despesa mensal inválido");
            command.ValidationResult.Errors[1].ErrorMessage.Should().Be("Campo status inválido para exclusão");
        }

        [Fact]
        public void DeleteDespesaMensalNaoDeveApresentarMensagemValidacaoDespesaMensal()
        {
            var command = new DeleteDespesaMensalCommandBuilder()
                .ComId(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComIdSalario(new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"))
                .ComStatus(false)
                .Instanciar();
            command.IsValid();

            command.ValidationResult.IsValid.Should().BeTrue();
        }
    }
}
