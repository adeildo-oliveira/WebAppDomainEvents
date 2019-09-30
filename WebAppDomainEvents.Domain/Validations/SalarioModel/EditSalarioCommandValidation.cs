namespace WebAppDomainEvents.Domain.Validations.SalarioModel
{
    public class EditSalarioCommandValidation : SalarioCommandBaseValidation
    {
        public EditSalarioCommandValidation()
        {
            RuleId();
            RulePagamento();
            RuleAdiantamento();
        }
    }
}
