namespace WebAppDomainEvents.Infra.Repository.ScriptsReadOnly
{
    public class ScriptsDespesaMensalReadOnly
    {
        public readonly static string QueryObterDespesaMensal =
            @"SELECT DISTINCT 
                A.Id, A.Descricao, A.Valor, A.Data, A.Status,
	            B.Id, B.Pagamento, B.Adiantamento, B.Status
            FROM DespesaMensal A (NOLOCK)
            INNER JOIN Salario B (NOLOCK) ON (A.SalarioId = B.Id)
            WHERE A.Status = 1";

        public readonly static string QueryObterDespesaMensalPorId =
            @"SELECT DISTINCT 
                A.Id, A.Descricao, A.Valor, A.Data, A.Status,
	            B.Id, B.Pagamento, B.Adiantamento, B.Status
            FROM DespesaMensal A (nolock)
            INNER JOIN Salario B ON (A.SalarioId = B.Id)
            WHERE A.Status = 1 AND A.Id = @ID";
    }
}
