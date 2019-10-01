namespace WebAppDomainEvents.Domain.Validations.DespesaMensal
{
    public class EditDespesaMensalCommandValidation : DespesaMensalCommandBaseValidation
    {
        public EditDespesaMensalCommandValidation()
        {
            RuleId();
            RuleDescricao();
            RuleValor();
            RuleData();
        }
    }
}