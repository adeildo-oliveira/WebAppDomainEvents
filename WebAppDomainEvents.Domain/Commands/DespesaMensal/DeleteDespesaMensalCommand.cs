using WebAppDomainEvents.Domain.Validations.DespesaMensal;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensal
{
    public class DeleteDespesaMensalCommand : CommandBaseDespesaMensal
    {
        public override bool IsValid()
        {
            ValidationResult = new DeleteDespesaMensalCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
