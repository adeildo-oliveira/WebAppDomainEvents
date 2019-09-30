namespace WebAppDomainEvents.Domain.Validations.SalarioModel
{
    public class DeleteSalarioCommandValidation : SalarioCommandBaseValidation
    {
        public DeleteSalarioCommandValidation()
        {
            RuleId();
            RuleStatus();
        }
    }
}
