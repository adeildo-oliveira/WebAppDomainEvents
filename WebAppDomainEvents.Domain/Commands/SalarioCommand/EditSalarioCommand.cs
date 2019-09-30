using WebAppDomainEvents.Domain.Validations.SalarioModel;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class EditSalarioCommand : CommandBaseSalario
    {
        public override bool IsValid()
        {
            ValidationResult = new EditSalarioCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
