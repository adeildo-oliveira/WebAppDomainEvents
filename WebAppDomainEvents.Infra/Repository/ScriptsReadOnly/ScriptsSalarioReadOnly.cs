namespace WebAppDomainEvents.Infra.Repository.ScriptsReadOnly
{
    public class ScriptsSalarioReadOnly
    {
        public readonly static string QueryObterSalario = 
            @"SELECT Id, Pagamento, Adiantamento, Status
            FROM Salario (nolock)
            WHERE Status = 1";

        public readonly static string QueryObterSalarioPorId =
            @"SELECT Id, Pagamento, Adiantamento, Status
              FROM Salario
              WHERE Id = @ID AND Status = 1";
    }
}
