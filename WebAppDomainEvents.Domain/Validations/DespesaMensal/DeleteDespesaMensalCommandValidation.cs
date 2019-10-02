namespace WebAppDomainEvents.Domain.Validations.DespesaMensal
{
    public class DeleteDespesaMensalCommandValidation : DespesaMensalCommandBaseValidation
    {
        public DeleteDespesaMensalCommandValidation()
        {
            RuleId();
            RuleIdSalario();
            RuleStatus();
        }
    }
}