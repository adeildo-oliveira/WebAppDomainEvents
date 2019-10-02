using WebAppDomainEvents.Domain.Validations.DespesaMensal;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensalCommand
{
    public class AddDespesaMensalCommand : CommandBaseDespesaMensal
    {
        public override bool IsValid()
        {
            ValidationResult = new AddDespesaMensalCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
