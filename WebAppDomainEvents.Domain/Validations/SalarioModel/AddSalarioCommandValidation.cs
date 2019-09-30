namespace WebAppDomainEvents.Domain.Validations.SalarioModel
{
    public class AddSalarioCommandValidation : SalarioCommandBaseValidation
    {
        public AddSalarioCommandValidation()
        {
            RuleAdiantamento();
            RulePagamento();
        }
    }
}
