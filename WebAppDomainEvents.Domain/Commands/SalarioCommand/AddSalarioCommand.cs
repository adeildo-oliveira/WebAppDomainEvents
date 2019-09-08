using WebAppDomainEvents.Domain.Validations.Salario;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class AddSalarioCommand : Command
    {
        public decimal Pagamento { get; set; }
        public decimal Adiantamento { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AddSalarioCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
