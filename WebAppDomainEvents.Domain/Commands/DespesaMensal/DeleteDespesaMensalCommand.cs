using System;
using WebAppDomainEvents.Domain.Validations.DespesaMensal;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensal
{
    public class DeleteDespesaMensalCommand : Command
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteDespesaMensalCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
