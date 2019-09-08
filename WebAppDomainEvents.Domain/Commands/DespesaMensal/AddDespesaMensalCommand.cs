using System;
using WebAppDomainEvents.Domain.Validations.DespesaMensal;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensal
{
    public class AddDespesaMensalCommand : Command
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AddDespesaMensalCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
