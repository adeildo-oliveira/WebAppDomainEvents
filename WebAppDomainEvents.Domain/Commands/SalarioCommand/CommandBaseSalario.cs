using System;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public abstract class CommandBaseSalario : Command
    {
        public Guid Id { get; set; }
        public decimal Pagamento { get; set; }
        public decimal Adiantamento { get; set; }
        public bool Status { get; set; }
    }
}
