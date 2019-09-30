using WebAppDomainEvents.Domain.Validations.SalarioModel;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class AddSalarioCommand : CommandBaseSalario
    {
        public override bool IsValid()
        {
            ValidationResult = new AddSalarioCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
