using System;

namespace WebApi.DomainEvents.Models.CommandsView.SalarioCommandView
{
    public class EditSalarioCommandView
    {
        public Guid Id { get; set; }
        public decimal Pagamento { get; set; }
        public decimal Adiantamento { get; set; }
    }
}
