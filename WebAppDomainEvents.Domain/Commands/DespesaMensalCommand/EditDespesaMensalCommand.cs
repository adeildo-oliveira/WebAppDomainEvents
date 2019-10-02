using WebAppDomainEvents.Domain.Validations.DespesaMensal;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensalCommand
{
    public class EditDespesaMensalCommand : CommandBaseDespesaMensal
    {
        public override bool IsValid()
        {
            ValidationResult = new EditDespesaMensalCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
