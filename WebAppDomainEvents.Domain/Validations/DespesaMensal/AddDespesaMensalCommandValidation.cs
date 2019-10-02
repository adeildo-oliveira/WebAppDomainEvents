namespace WebAppDomainEvents.Domain.Validations.DespesaMensal
{
    public class AddDespesaMensalCommandValidation : DespesaMensalCommandBaseValidation
    {
        public AddDespesaMensalCommandValidation()
        {
            RuleDescricao();
            RuleIdSalario();
            RuleValor();
            RuleData();
        }
    }
}