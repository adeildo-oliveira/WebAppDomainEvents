using System;
using WebAppDomainEvents.Domain.Validations.Salario;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class EditSalarioCommand : AddSalarioCommand
    {
        public Guid Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new EditSalarioCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
