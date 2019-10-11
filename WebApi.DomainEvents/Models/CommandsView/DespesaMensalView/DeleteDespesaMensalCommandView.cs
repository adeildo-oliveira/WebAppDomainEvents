using System;

namespace WebApi.DomainEvents.Models.CommandsView.DespesaMensalView
{
    public class DeleteDespesaMensalCommandView
    {
        public Guid Id { get; set; }
        public Guid IdSalario { get; set; }
        public bool Status { get; set; }
    }
}
