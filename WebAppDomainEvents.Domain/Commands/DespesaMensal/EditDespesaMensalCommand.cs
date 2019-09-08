using System;
using WebAppDomainEvents.Domain.Validations.DespesaMensal;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensal
{
    public class EditDespesaMensalCommand : AddDespesaMensalCommand
    {
        public Guid Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new EditDespesaMensalCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
