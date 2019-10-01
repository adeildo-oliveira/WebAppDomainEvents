namespace WebAppDomainEvents.Domain.Validations.DespesaMensal
{
    public class AddDespesaMensalCommandValidation : DespesaMensalCommandBaseValidation
    {
        public AddDespesaMensalCommandValidation()
        {
            RuleDescricao();
            RuleValor();
            RuleData();
        }
    }
}