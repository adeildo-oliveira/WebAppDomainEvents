using System;
using WebAppDomainEvents.Domain.Validations.SalarioModel;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class DeleteSalarioCommand : CommandBaseSalario
    {
        public override bool IsValid()
        {
            ValidationResult = new DeleteSalarioCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
